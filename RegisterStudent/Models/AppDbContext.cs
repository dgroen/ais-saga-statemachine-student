using Microsoft.EntityFrameworkCore;

namespace RegisterStudent.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<StudentInfo> StudentInfo { get; set; }
    }
}
