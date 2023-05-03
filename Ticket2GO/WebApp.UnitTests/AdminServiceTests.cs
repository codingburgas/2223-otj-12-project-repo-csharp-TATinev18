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

        [Test]
        public async Task GetUser_ValidUserId_ReturnsUser()
        {
            var user = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com", FirstName = "Test", LastName = "User" };
            await _userManager.CreateAsync(user);

            var result = await _adminService.GetUser(user.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(result.LastName, Is.EqualTo(user.LastName));
        }

        [Test]
        public async Task GetUserInformation_ValidUser_ReturnsUserInformation()
        {
            var user = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com", FirstName = "Test", LastName = "User" };
            await _userManager.CreateAsync(user);

            var role = new IdentityRole { Name = "Admin" };
            await _roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(user, role.Name);

            var result = await _adminService.GetUserInformation(user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(user.Id));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(result.LastName, Is.EqualTo(user.LastName));
            Assert.That(result.Roles, Is.Not.Null);
            Assert.That(result.Roles, Has.Member(role.Name));
            Assert.That(result.AvailableRoles, Is.Not.Null);
            Assert.That(result.AvailableRoles, Has.Member(role.Name));
        }

        [Test]
        public async Task EditUser_ValidInput_UpdatesUser()
        {
            var user = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com", FirstName = "Test", LastName = "User" };
            await _userManager.CreateAsync(user);

            var updatedModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Email = "updated@test.com",
                FirstName = "Updated",
                LastName = "User"
            };

            var updateResult = await _adminService.EditUser(updatedModel, user);
            var updatedUser = await _userManager.FindByIdAsync(user.Id);

            Assert.That(updateResult.Succeeded, Is.True);
            Assert.That(updatedUser, Is.Not.Null);
            Assert.That(updatedUser.Id, Is.EqualTo(updatedModel.UserId));
            Assert.That(updatedUser.Email, Is.EqualTo(updatedModel.Email));
            Assert.That(updatedUser.UserName, Is.EqualTo(updatedModel.Email));
            Assert.That(updatedUser.FirstName, Is.EqualTo(updatedModel.FirstName));
            Assert.That(updatedUser.LastName, Is.EqualTo(updatedModel.LastName));
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}