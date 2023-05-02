using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    public class TicketServiceTests
    {
        private ApplicationDbContext _context;
        private TicketService _ticketService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _ticketService = new TicketService(_context);
        }

        [Test]
        public async Task GetTickets_ValidInput_ReturnsFilteredTickets()
        {
            var transportCompany = new TransportCompany { Name = "Test Company", Logo = Encoding.UTF8.GetBytes("test-logo.png") };
            _context.TransportCompanies.Add(transportCompany);

            var bus = new Bus { Name = "Test Bus", SeatsNumber = 50, TransportCompany = transportCompany };
            _context.Buses.Add(bus);

            var destination = new Destination
            {
                StartingDestination = "A",
                FinalDestination = "B",
                Departure = DateTime.Now.AddDays(1),
                TimeOfArrival = DateTime.Now.AddDays(1).AddHours(4),
                Price = 30.0m,
                Bus = bus
            };
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            var result = await _ticketService.GetTickets(transportCompany.TransportCompanyId.ToString(), "A", "B", null);

            Assert.IsNotNull(result);
            Assert.That(result.Destinations.Count, Is.EqualTo(1));
            var firstResult = result.Destinations.First();
            Assert.That(firstResult.DestinationId, Is.EqualTo(destination.DestinationId));
            Assert.That(firstResult.StartingDestination, Is.EqualTo(destination.StartingDestination));
            Assert.That(firstResult.FinalDestination, Is.EqualTo(destination.FinalDestination));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}