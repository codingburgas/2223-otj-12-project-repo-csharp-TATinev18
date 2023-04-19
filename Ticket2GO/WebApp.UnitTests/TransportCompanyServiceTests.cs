using System.Threading.Tasks;
using NUnit.Framework;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace WebApp.Tests
{
    public class TransportCompanyServiceTests
    {
        private ApplicationDbContext _context;
        private ITransportCompanyService _transportCompanyService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            _transportCompanyService = new TransportCompanyService(_context);
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
                Name = "", // Invalid input: empty name
                Logo = null // Invalid input: null logo
            };

            await _transportCompanyService.CreateTransportCompany(viewModel, user);

            var transportCompany = await _context.TransportCompanies.FirstOrDefaultAsync(tc => tc.Name == viewModel.Name);
            Assert.IsNull(transportCompany);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
