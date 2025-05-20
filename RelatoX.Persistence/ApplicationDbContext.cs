using Microsoft.EntityFrameworkCore;
using RelatoX.Domain.Entities;

namespace RelatoX.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ConsumptionEntry> Consumptions { get; set; }

        public ApplicationDbContext(DbContextOptions options)
     : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}