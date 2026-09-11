using CavistaLaptopLifecycleManagement.Api.Database.Entities;
using Immediate.Injections.Shared;
using Microsoft.EntityFrameworkCore;

namespace CavistaLaptopLifecycleManagement.Api.Database
{
    [RegisterTransient]
    public sealed partial class CLMDbContext(DbContextOptions<CLMDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Laptop> Laptops { get; set; }

        public DbSet<LaptopHistory> LaptopHistories { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<AuditTrail> AuditTrails { get; set; }

        public DbSet<TicketHistory> TicketHistories { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Laptop>().ToTable("UserLaptops");
            modelBuilder.Entity<LaptopHistory>().ToTable("LaptopHistories");
            modelBuilder.Entity<Ticket>().ToTable("Tickets");
            modelBuilder.Entity<AuditTrail>().ToTable("AuditTrails");

            modelBuilder.Entity<LaptopHistory>().
                    HasIndex(laptopHis => laptopHis.UserLaptopID, "Idx_LaptopHistory_LaptopId");

            modelBuilder.Entity<Notification>().
                    HasIndex(notification => notification.UserId, "Idx_Notification_UserId");

            modelBuilder.Entity<Ticket>().
                    HasIndex(ticket => ticket.UserId, "Idx_Ticket_UserId");
            modelBuilder.Entity<Ticket>().
                    HasIndex(ticket => ticket.LaptopId, "Idx_LaptopId_UserId");
            modelBuilder.Entity<Ticket>().
                    HasIndex(ticket => ticket.TicketNumber, "Idx_TicketNumber_UserId");

            modelBuilder.Entity<TicketComment>().
                  HasIndex(ticketComment => ticketComment.TicketId, "Idx_TicketComment_TicketId");

            modelBuilder.Entity<TicketHistory>().
                    HasIndex(tickethistory => tickethistory.TicketID, "Idx_TicketHistory_TicketID");

            modelBuilder.Entity<User>().
                    HasIndex(user => user.Auth0UserId, "Idx_User_Auth0UserId");

            modelBuilder.Entity<Laptop>().
                    HasIndex(userLaptop => userLaptop.UserId, "Idx_Laptop_UserId");
        }
    }
}
