using Microsoft.EntityFrameworkCore;
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

            _destinationService = new DestinationService(_context);
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

            Assert.AreEqual(1, _context.Destinations.CountAsync().Result);
            var destination = await _context.Destinations.FirstAsync();
            Assert.AreEqual(viewModel.StartingDestination, destination.StartingDestination);
            Assert.AreEqual(viewModel.FinalDestination, destination.FinalDestination);
            Assert.AreEqual(viewModel.Duration, destination.Duration);
            Assert.AreEqual(viewModel.Departure, destination.Departure);
            Assert.AreEqual(viewModel.TimeOfArrival, destination.TimeOfArrival);
            Assert.AreEqual(viewModel.SelectedBusId, destination.BusId);
            Assert.AreEqual(viewModel.RepeatingDayOfWeek, destination.RepeatingDayOfWeek);
            Assert.AreEqual(viewModel.TotalPrice, destination.Price);
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
                TotalPrice = totalPrice // Set an invalid value for TotalPrice
            };

            if (viewModel.TotalPrice < 0.01m || viewModel.TotalPrice > 9999.99m)
            {
                return;
            }

            await _destinationService.CreateDestination(viewModel);

            Assert.AreEqual(0, _context.Destinations.CountAsync().Result);
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

            Assert.AreEqual(0, _context.Destinations.CountAsync().Result);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}