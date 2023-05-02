using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    public class AdminServiceTests
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AdminService _adminService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            var userStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

            var roleStore = new RoleStore<IdentityRole>(_context);
            _roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);

            _adminService = new AdminService(_userManager, _roleManager, _context);
        }

        [Test]
        public async Task GetUsers_ValidInput_ReturnsUsers()
        {
            var user = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com", FirstName = "Test", LastName = "User" };
            await _userManager.CreateAsync(user);

            var userRoleViewModels = await _adminService.GetUsers();

            Assert.That(userRoleViewModels.Count, Is.EqualTo(1));
            var viewModel = userRoleViewModels.First();
            Assert.That(viewModel.UserId, Is.EqualTo(user.Id));
            Assert.That(viewModel.Email, Is.EqualTo(user.Email));
            Assert.That(viewModel.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(viewModel.LastName, Is.EqualTo(user.LastName));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}