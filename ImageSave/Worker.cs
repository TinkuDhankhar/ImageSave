using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Html;
using System.Reflection.PortableExecutable;
using System.Text;
namespace ImageSave
{
    public class Worker : BackgroundService
    {
        //Install System.Drawing.Common
        private readonly ILogger<Worker> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, ApplicationDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string SQL = "SELECT * FROM ImageProcess WHERE Active = 1 AND (SyncDateTime IS NULL OR SyncDateTime = CONVERT(DATETIME, @SyncDate))";
                var data = await _db.ImageProcess.FromSqlRaw(SQL, new SqlParameter("@SyncDate", DateTime.Now)).ToListAsync();
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    string newHTML = GetHTML(data);
                    foreach (var item in data)
                    {
                        ReduceImageSize(0.5, item.SourcePath, item.DestinationPath);
                        //_db.Database.ExecuteSqlRaw("UPDATE ImageProcess SET SyncDateTime = @SyncDate WHERE Id = @Id", new SqlParameter("@SyncDate", DateTime.Now), new SqlParameter("@Id", item.Id));

                        //SendMail(new EmailModel() { Attachment = item.DestinationPath, Body = "This is test Mail", To = "tinkudhankhar@hotmail.com" });
                    }
                    string path = Download(newHTML, "Test");
                    SendMail(new EmailModel() { Attachment = path, Body = "This is test mail for you...", To = "tinkudhankhar@hotmail.com" });
                    File.Delete(path);
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
        private void ReduceImageSize(double scaleFactor, string sourcePath, string targetPath)
        {
            try
            {
                using var image = Image.FromFile(sourcePath);
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private bool SendMail(EmailModel model)
        {
            var smtpSettings = _configuration.GetSection("smtpSettings");
            try
            {
                using (MailMessage mm = new MailMessage())
                {
                    string[] ar = model.To.Split(';');
                    foreach (string s in ar)
                    {
                        if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                        {
                            mm.To.Add(s);
                        }
                    }
                    mm.From = new MailAddress(smtpSettings["SmtpMailFrom"], smtpSettings["SmtpName"]);
                    mm.Priority = MailPriority.High;
                    mm.Subject = smtpSettings["Subject"];
                    mm.Body = model.Body;
                    mm.Attachments.Add(new Attachment(model.Attachment, MediaTypeNames.Application.Octet));
                    mm.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = smtpSettings["SmtpHost"];
                        smtp.EnableSsl = Convert.ToBoolean(smtpSettings["EnableSsl"]);
                        NetworkCredential NetworkCred = new NetworkCredential(smtpSettings["SmtpMailFrom"], smtpSettings["Password"]);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = Convert.ToInt32(smtpSettings["SmtpPort"]);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(mm);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string Download(string html, string FileName)
        {
            FileName = FileName + DateTime.Now.ToString("ddMMyyyyHHmmssFFFFFFF");
            var htmlToPdf = new HtmlToPDFCore.HtmlToPDF();
            htmlToPdf.Margins = new HtmlToPDFCore.PageMargins(10, 0, 10, 0);
            var pdf = htmlToPdf.ReturnPDF(html);
            var pathToImage = Path.Combine(Directory.GetCurrentDirectory(), $"{FileName}.pdf");
            System.IO.File.Delete(pathToImage);
            System.IO.File.WriteAllBytesAsync(pathToImage, pdf);
            return pathToImage;
        }
        private string GetHTML(List<ImageProcess> imageProcess)
        {
            string NewText = string.Empty;
            string html = File.ReadAllText("Test.html");
            foreach (var item in imageProcess)
            {
                NewText += $"<tr><td><img src='{item.DestinationPath}'/></td><td>{item.CreatedDate}</td></tr>";
            }
            html = html.Replace("{body}", NewText);
            return html;
        }
    }
}