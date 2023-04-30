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
                Logo = null
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
            Assert.AreEqual(transportCompany.TransportCompanyId, result.TransportCompanyId);
            Assert.AreEqual(user.Id, result.ApplicationUserId);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
