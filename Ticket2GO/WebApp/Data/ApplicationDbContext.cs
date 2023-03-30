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
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.FirstName).HasMaxLength(255);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LastName).HasMaxLength(255);

            modelBuilder.Entity<TicketDestination>()
                .HasKey(td => new { td.TicketId, td.DestinationId });

            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Bus)
                .WithMany()
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransportCompanyAspNetUser>()
                .HasKey(tcu => new { tcu.TransportCompanyId, tcu.ApplicationUserId });

            modelBuilder.Entity<TransportCompanyAspNetUser>()
                .HasOne(tcu => tcu.TransportCompany)
                .WithMany(tc => tc.TransportCompanyAspNetUsers)
                .HasForeignKey(tcu => tcu.TransportCompanyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransportCompanyAspNetUser>()
                .HasOne(tcu => tcu.ApplicationUser)
                .WithMany(au => au.TransportConpaniesAspNetUsers)
                .HasForeignKey(tcu => tcu.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<TransportCompany> TransportCompanies { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketDestination> TicketsDestinations { get; set; }
        public DbSet<TransportCompanyAspNetUser> TransportCompaniesAspNetUsers { get; set; }
    }
}