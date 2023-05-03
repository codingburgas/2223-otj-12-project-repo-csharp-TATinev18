using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Tests
{
    public class TransportCompanyServiceTests
    {
        private ApplicationDbContext _context;
        private ITransportCompanyService _transportCompanyService;
        private UserManager<ApplicationUser> _userManager;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication();

            _serviceProvider = services.BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            _transportCompanyService = new TransportCompanyService(_context);

            _userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        }

        [Test]
        public async Task CreateTransportCompany_ValidInput_CreatesTransportCompany()
        {
            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a test file."));
            var viewModel = new TransportCompanyViewModel
            {
                Name = "Test Transport Company",
                Logo = new FormFile(stream, 0, stream.Length, "testLogo.jpg", "image/jpeg")
            };

            await _transportCompanyService.CreateTransportCompany(viewModel, user);

            var transportCompany = await _context.TransportCompanies.FirstOrDefaultAsync(tc => tc.Name == viewModel.Name);
            Assert.IsNotNull(transportCompany);
        }

        [Test]
        public async Task CreateTransportCompany_InvalidInput_CreatesTransportCompany()
        {
            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var viewModel = new TransportCompanyViewModel
            {
                Name = "",
                Logo = null!
            };

            await _transportCompanyService.CreateTransportCompany(viewModel, user);

            var transportCompany = await _context.TransportCompanies.FirstOrDefaultAsync(tc => tc.Name == viewModel.Name);
            Assert.IsNull(transportCompany);
        }

        [Test]
        public async Task GetCurrentUsersTransportCompany_ReturnsCorrectTransportCompany()
        {
            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            await _userManager.CreateAsync(user, "Test@123");

            var transportCompany = new TransportCompany
            {
                Name = "Test Transport Company",
                Logo = new byte[] { 1, 2, 3 },
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now
            };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var transportCompanyAspNetUser = new TransportCompanyAspNetUser
            {
                TransportCompanyId = transportCompany.TransportCompanyId,
                ApplicationUserId = user.Id
            };
            _context.TransportCompaniesAspNetUsers.Add(transportCompanyAspNetUser);
            await _context.SaveChangesAsync();

            var transportCompanyService = new TransportCompanyService(_context);

            var result = transportCompanyService.GetCurrentUsersTransportCompany(user);

            Assert.IsNotNull(result);
            Assert.That(result.TransportCompanyId, Is.EqualTo(transportCompany.TransportCompanyId));
            Assert.That(result.ApplicationUserId, Is.EqualTo(user.Id));
        }

        [Test]
        public async Task CreateBus_ValidInput_CreatesBus()
        {
            var transportCompanyId = Guid.NewGuid();
            var viewModel = new CreateBusViewModel
            {
                Name = "Test Bus",
                SeatsNumber = 42
            };

            await _transportCompanyService.CreateBus(transportCompanyId, viewModel);

            var bus = await _context.Buses.FirstOrDefaultAsync(b => b.Name == viewModel.Name);
            Assert.IsNotNull(bus);
            Assert.That(viewModel.SeatsNumber, Is.EqualTo(bus.SeatsNumber));
            Assert.That(transportCompanyId, Is.EqualTo(bus.TransportCompanyId));
        }

        [Test]
        public async Task GetTranspoerCompanyDeatils_ValidId_ReturnsTransportCompany()
        {
            var transportCompany = new TransportCompany
            {
                Name = "Test Transport Company",
                Logo = new byte[] { 1, 2, 3 },
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now
            };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var result = await _transportCompanyService.GetTranspoerCompanyDeatils(transportCompany.TransportCompanyId);

            Assert.IsNotNull(result);
            Assert.That(result.TransportCompanyId, Is.EqualTo(transportCompany.TransportCompanyId));
        }

        [Test]
        public async Task TransportCompanyExists_ValidId_ReturnsTrue()
        {
            var transportCompany = new TransportCompany
            {
                Name = "Test Transport Company",
                Logo = new byte[] { 1, 2, 3 },
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now
            };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var result = _transportCompanyService.TransportCompanyExists(transportCompany.TransportCompanyId);

            Assert.IsTrue(result);
        }

        [Test]
        public void TransportCompanyExists_InvalidId_ReturnsFalse()
        {
            var invalidId = Guid.NewGuid();

            var result = _transportCompanyService.TransportCompanyExists(invalidId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetTransportCompany_ValidId_ReturnsTransportCompany()
        {
            var transportCompany = new TransportCompany
            {
                Name = "Test Transport Company",
                Logo = new byte[] { 1, 2, 3 },
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now
            };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var result = await _transportCompanyService.GetTransportCompany(transportCompany.TransportCompanyId);

            Assert.IsNotNull(result);
            Assert.That(transportCompany.TransportCompanyId, Is.EqualTo(result.TransportCompanyId));
        }

        [Test]
        public async Task GetTransportCompany_InvalidId_ReturnsNull()
        {
            var invalidId = Guid.NewGuid();

            var result = await _transportCompanyService.GetTransportCompany(invalidId);

            Assert.IsNull(result);
        }

        [Test]
        public void GenerateTransportCompanyViewModel_ValidTransportCompany_ReturnsViewModel()
        {
            var transportCompany = new TransportCompany
            {
                TransportCompanyId = Guid.NewGuid(),
                Name = "Test Transport Company"
            };

            var result = _transportCompanyService.GenerateTransportCompnayViewModel(transportCompany);

            Assert.IsNotNull(result);
            Assert.That(transportCompany.TransportCompanyId, Is.EqualTo(result.TransportCompanyId));
            Assert.That(transportCompany.Name, Is.EqualTo(result.Name));
        }

        [Test]
        public async Task EditTransportCompany_ValidInput_UpdatesTransportCompany()
        {
            var transportCompany = new TransportCompany
            {
                Name = "Test Transport Company",
                Logo = new byte[] { 1, 2, 3 }
            };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var newLogoStream = new MemoryStream(Encoding.UTF8.GetBytes("This is a new logo."));
            var viewModel = new TransportCompanyViewModel
            {
                TransportCompanyId = transportCompany.TransportCompanyId,
                Name = "Updated Transport Company",
                Logo = new FormFile(newLogoStream, 0, newLogoStream.Length, "newLogo.jpg", "image/jpeg")
            };

            await _transportCompanyService.EditTransportCompany(viewModel, transportCompany);

            var updatedTransportCompany = await _context.TransportCompanies.FindAsync(transportCompany.TransportCompanyId);
            Assert.IsNotNull(updatedTransportCompany);
            Assert.That(viewModel.Name, Is.EqualTo(updatedTransportCompany.Name));
            Assert.That(viewModel.Logo.Length, Is.EqualTo(updatedTransportCompany.Logo.Length));
        }

        [Test]
        public async Task GetAllTransportCompanies_ReturnsAllTransportCompanies()
        {
            var transportCompany1 = new TransportCompany { Name = "Test Transport Company 1", Logo = new byte[] { 1, 2, 3 } };
            var transportCompany2 = new TransportCompany { Name = "Test Transport Company 2", Logo = new byte[] { 4, 5, 6 } };

            _context.TransportCompanies.AddRange(transportCompany1, transportCompany2);
            await _context.SaveChangesAsync();

            var allTransportCompanies = await _transportCompanyService.GetAllTransportCompanies();

            Assert.IsNotNull(allTransportCompanies);
            Assert.That(allTransportCompanies.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetDestinations_ReturnsCorrectDestinations()
        {
            var transportCompany = new TransportCompany { Name = "Test Transport Company", Logo = new byte[] { 1, 2, 3 } };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var bus = new Bus { Name = "Test Bus", SeatsNumber = 10, TransportCompanyId = transportCompany.TransportCompanyId };
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            var destination1 = new Destination
            {
                BusId = bus.BusId,
                TimeOfArrival = DateTime.Now.AddHours(1),
                Departure = DateTime.Now,
                FinalDestination = "City 1",
                StartingDestination = "City 2"
            };
            var destination2 = new Destination
            {
                BusId = bus.BusId,
                TimeOfArrival = DateTime.Now.AddHours(2),
                Departure = DateTime.Now.AddHours(1),
                FinalDestination = "City 3",
                StartingDestination = "City 4"
            };
            _context.Destinations.AddRange(destination1, destination2);
            await _context.SaveChangesAsync();

            var destinations = await _transportCompanyService.GetDestinations(transportCompany.TransportCompanyId);

            Assert.IsNotNull(destinations);
            Assert.That(destinations.Count, Is.EqualTo(2));
            Assert.IsTrue(destinations.Any(d => d.DestinationId == destination1.DestinationId));
            Assert.IsTrue(destinations.Any(d => d.DestinationId == destination2.DestinationId));
        }

        [Test]
        public async Task DeleteTransportCompany_ValidInput_DeletesTransportCompany()
        {
            var transportCompany = new TransportCompany { Name = "Test Transport Company", Logo = new byte[] { 1, 2, 3 } };
            _context.TransportCompanies.Add(transportCompany);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var transportCompanyAspNetUser = new TransportCompanyAspNetUser
            {
                TransportCompanyId = transportCompany.TransportCompanyId,
                ApplicationUserId = user.Id
            };
            _context.TransportCompaniesAspNetUsers.Add(transportCompanyAspNetUser);
            await _context.SaveChangesAsync();

            await _transportCompanyService.DeleteTransportCompany(transportCompany.TransportCompanyId, transportCompany);

            var deletedTransportCompany = await _context.TransportCompanies.FindAsync(transportCompany.TransportCompanyId);
            Assert.IsNull(deletedTransportCompany);

            var deletedTransportCompanyAspNetUser = await _context.TransportCompaniesAspNetUsers
                .FirstOrDefaultAsync(tc => tc.TransportCompanyId == transportCompany.TransportCompanyId);
            Assert.IsNull(deletedTransportCompanyAspNetUser);
        }

        [Test]
        public async Task GetCompanyBuses_ReturnsBusesForTransportCompany()
        {
            var transportCompanyId = Guid.NewGuid();
            var expectedBuses = new List<Bus>
            {
                new Bus { Name = "Bus 1", SeatsNumber = 50, TransportCompanyId = transportCompanyId },
                new Bus { Name = "Bus 2", SeatsNumber = 30, TransportCompanyId = transportCompanyId },
                new Bus { Name = "Bus 3", SeatsNumber = 40, TransportCompanyId = Guid.NewGuid() } 
            };
            _context.Buses.AddRange(expectedBuses);
            await _context.SaveChangesAsync();

            var result = _transportCompanyService.GetCompanyBuses(transportCompanyId);

            Assert.IsNotNull(result);
            var actualBuses = await result.ToListAsync();
            Assert.That(actualBuses.Count, Is.EqualTo(2));
            Assert.IsTrue(actualBuses.Any(b => b.Name == "Bus 1"));
            Assert.IsTrue(actualBuses.Any(b => b.Name == "Bus 2"));
            Assert.IsFalse(actualBuses.Any(b => b.Name == "Bus 3"));
        }

        [Test]
        public async Task GetTransportCompanies_ReturnsTransportCompaniesForUser()
        {
            // Arrange
            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var transportCompany1 = new TransportCompany { Name = "Test Transport Company 1", Logo = new byte[] { 1, 2, 3 } };
            var transportCompany2 = new TransportCompany { Name = "Test Transport Company 2", Logo = new byte[] { 4, 5, 6 } };

            _context.TransportCompaniesAspNetUsers.AddRange(
                new TransportCompanyAspNetUser { TransportCompanyId = transportCompany1.TransportCompanyId, ApplicationUserId = user.Id },
                new TransportCompanyAspNetUser { TransportCompanyId = transportCompany2.TransportCompanyId, ApplicationUserId = "otherUserId" });

            _context.TransportCompanies.AddRange(transportCompany1, transportCompany2);
            await _context.SaveChangesAsync();

            var transportCompanies = await _transportCompanyService.GetTransportCompanies(user);

            Assert.That(transportCompanies.Count, Is.EqualTo(1));
            Assert.That(transportCompanies[0].Name, Is.EqualTo("Test Transport Company 1"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
