using Microsoft.EntityFrameworkCore;
using SagaStateMachine;

namespace SagaService.Models
{
    public class SagaContext : DbContext
    {
       
        public SagaContext(DbContextOptions<SagaContext> options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentStateData>().HasKey(x => x.CorrelationId);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<StudentStateData> StudentStateData{ get;set; }
    }
}
