using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Models;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TicketDestination>()
                .HasKey(td => new { td.TicketId, td.DestinationId });

            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Bus)
                .WithMany()
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<TransportCompany> TransportCompany { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketDestination> TicketsDestinations { get; set; }
    }
}