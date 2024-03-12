using Microsoft.EntityFrameworkCore;

namespace ImageSave
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<ImageProcess> ImageProcess {  get; set; }
    }
}