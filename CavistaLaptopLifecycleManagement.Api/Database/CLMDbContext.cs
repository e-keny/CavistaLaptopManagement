using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Database
{
    [RegisterTransient]
    public sealed partial class CLMDbContext(DbContextOptions<CLMDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserLaptop> UserLaptops { get; set; }

        public DbSet<LaptopHistory> LaptopHistories { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<AuditTrail> AuditTrails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserLaptop>().ToTable("UserLaptops");
            modelBuilder.Entity<LaptopHistory>().ToTable("LaptopHistories");
            modelBuilder.Entity<Ticket>().ToTable("Tickets");
            modelBuilder.Entity<AuditTrail>().ToTable("AuditTrails");
        }
    }
}
