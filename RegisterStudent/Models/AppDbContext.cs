using Microsoft.EntityFrameworkCore;

namespace RegisterStudent.Models
{
    public class AppDbContext : DbContext
    {


        public virtual DbSet<StudentInfo> StudentInfo { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public AppDbContext() : base(new DbContextOptions<AppDbContext>()) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
