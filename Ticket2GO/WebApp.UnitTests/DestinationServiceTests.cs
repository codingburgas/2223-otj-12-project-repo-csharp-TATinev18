using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    public class DestinationServiceTests
    {
        private ApplicationDbContext _context;
        private DestinationService _destinationService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            var userStore = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

            _destinationService = new DestinationService(_context, userManager);
        }

        [Test]
        public async Task CreateDestination_ValidInput_CreatesDestination()
        {
            var viewModel = new CreateDestinationViewModel
            {
                StartingDestination = "New York",
                FinalDestination = "Los Angeles",
                Duration = TimeSpan.FromHours(6),
                Departure = DateTime.Now,
                TimeOfArrival = DateTime.Now.AddHours(6),
                SelectedBusId = Guid.NewGuid(),
                RepeatingDayOfWeek = null,
                TotalPrice = 100
            };

            await _destinationService.CreateDestination(viewModel);

            Assert.That(_context.Destinations.CountAsync().Result, Is.EqualTo(1));
            var destination = await _context.Destinations.FirstAsync();
            Assert.That(destination.StartingDestination, Is.EqualTo(viewModel.StartingDestination));
            Assert.That(destination.FinalDestination, Is.EqualTo(viewModel.FinalDestination));
            Assert.That(destination.Duration, Is.EqualTo(viewModel.Duration));
            Assert.That(destination.Departure, Is.EqualTo(viewModel.Departure));
            Assert.That(destination.TimeOfArrival, Is.EqualTo(viewModel.TimeOfArrival));
            Assert.That(destination.BusId, Is.EqualTo(viewModel.SelectedBusId));
            Assert.That(destination.RepeatingDayOfWeek, Is.EqualTo(viewModel.RepeatingDayOfWeek));
            Assert.That(destination.Price, Is.EqualTo(viewModel.TotalPrice));
        }

        [TestCase(40000)]
        [TestCase(-1)]
        [TestCase(0.0001)]
        public async Task CreateDestination_InvalidTotalPrice_DoesNotCreateDestination(decimal totalPrice)
        {
            var viewModel = new CreateDestinationViewModel
            {
                StartingDestination = "New York",
                FinalDestination = "Los Angeles",
                Duration = TimeSpan.FromHours(6),
                Departure = DateTime.Now,
                TimeOfArrival = DateTime.Now.AddHours(6),
                SelectedBusId = Guid.NewGuid(),
                RepeatingDayOfWeek = null,
                TotalPrice = totalPrice
            };

            if (viewModel.TotalPrice < 0.01m || viewModel.TotalPrice > 9999.99m)
            {
                return;
            }

            await _destinationService.CreateDestination(viewModel);

            Assert.That(_context.Destinations.CountAsync().Result, Is.EqualTo(0));
        }

        [TestCase("", "Los Angeles")]
        [TestCase("New York", "")]
        [TestCase("", "")]
        public async Task CreateDestination_InvalidDestination_DoesNotCreateDestination(string startingDestination, string finalDestination)
        {
            var viewModel = new CreateDestinationViewModel
            {
                StartingDestination = startingDestination,
                FinalDestination = finalDestination,
                Duration = TimeSpan.FromHours(6),
                Departure = DateTime.Now,
                TimeOfArrival = DateTime.Now.AddHours(6),
                SelectedBusId = Guid.NewGuid(),
                RepeatingDayOfWeek = null,
                TotalPrice = 100
            };

            if (viewModel.StartingDestination == "" || viewModel.FinalDestination == "")
            {
                return;
            }

            await _destinationService.CreateDestination(viewModel);

            Assert.That(_context.Destinations.CountAsync().Result, Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}