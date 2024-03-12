using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
namespace ImageSave
{
    public class Worker : BackgroundService
    {
        //Install System.Drawing.Common
        private readonly ILogger<Worker> _logger;
        private readonly ApplicationDbContext _db;

        public Worker(ILogger<Worker> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string SQL = "SELECT * FROM ImageProcess WHERE Active = 1 AND (SyncDateTime IS NULL OR SyncDateTime = CONVERT(DATE, @SyncDate))";
                var data = await _db.ImageProcess.FromSqlRaw(SQL, new SqlParameter("@SyncDate", DateTime.Now.ToShortDateString())).ToListAsync();
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    foreach (var item in data)
                    {
                        ReduceImageSize(0.5, item.SourcePath, item.DestinationPath);
                        _db.Database.ExecuteSqlRaw("UPDATE ImageProcess SET SyncDateTime = @SyncDate WHERE Id = @Id", new SqlParameter("@SyncDate", DateTime.Now), new SqlParameter("@Id", item.Id));
                    }
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
    }
}
