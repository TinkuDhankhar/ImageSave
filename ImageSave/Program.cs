using Microsoft.EntityFrameworkCore;

namespace ImageSave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder1 = Host.CreateApplicationBuilder(args);
            var builder = Host.CreateDefaultBuilder(args);
            builder.UseWindowsService(w =>
            {
                w.ServiceName = "ImageSave.WorkerService";
            })
                .ConfigureServices(s =>
                {
                    s.AddHostedService<Worker>();
                    if (OperatingSystem.IsWindows())
                    {
                    }
                    s.AddDbContext<ApplicationDbContext>(x =>
                    {
                        x.UseSqlServer(builder1.Configuration.GetConnectionString("img"), b => b.MigrationsAssembly("img"));
                        x.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    }, ServiceLifetime.Singleton);
                });
            var host = builder.Build();
            host.Run();
        }
    }
}