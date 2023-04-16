using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Areas.Identity.Data;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    public class DestinationControllerTests
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            // Register services
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication();

            _serviceProvider = services.BuildServiceProvider();

            // Get required services for UserManager and HttpContextAccessor
            _userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _httpContextAccessor = new HttpContextAccessor();
        }

        [Test]
        public async Task CreateDestination_ValidInput_CreatesDestination()
        {
            // Create and set up the ApplicationDbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            var httpContext = new DefaultHttpContext();
            _httpContextAccessor.HttpContext = httpContext;
            httpContext.RequestServices = _serviceProvider;

            // Arrange
            var controller = new DestinationController(_context, _userManager)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal(new ClaimsIdentity()) }
                }
            };

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

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual(1, _context.Destinations.CountAsync().Result);
            var destination = _context.Destinations.FirstAsync().Result;
            Assert.AreEqual(viewModel.StartingDestination, destination.StartingDestination);
            Assert.AreEqual(viewModel.FinalDestination, destination.FinalDestination);
            Assert.AreEqual(viewModel.Duration, destination.Duration);
            Assert.AreEqual(viewModel.Departure, destination.Departure);
            Assert.AreEqual(viewModel.TimeOfArrival, destination.TimeOfArrival);
            Assert.AreEqual(viewModel.SelectedBusId, destination.BusId);
            Assert.AreEqual(viewModel.RepeatingDayOfWeek, destination.RepeatingDayOfWeek);
            Assert.AreEqual(viewModel.TotalPrice, destination.Price);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up any resources that were used in the test
            _context.Database.EnsureDeleted();
        }
    }
}
