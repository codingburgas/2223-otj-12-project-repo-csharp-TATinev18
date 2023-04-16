using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using WebApp.Areas.Identity.Data;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.ViewModels;

namespace WebApp.Tests
{
    public class TransportCompanyControllerTests
    {
        private IUserConfirmation<ApplicationUser> _userConfirmation;
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;
        private IOptions<IdentityOptions> _identityOptions;
        private ILogger<SignInManager<ApplicationUser>> _logger;
        private IAuthenticationSchemeProvider _schemes;
        private IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            // Create a new instance of the ApplicationDbContext
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);

            // Create a new instance of the HttpContextAccessor
            _httpContextAccessor = new HttpContextAccessor();

            // Register services
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication();

            _serviceProvider = services.BuildServiceProvider();

            // Get required services for UserManager, RoleManager, and SignInManager
            _userManager = services.BuildServiceProvider().GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
            _identityOptions = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityOptions>>();
            _logger = services.BuildServiceProvider().GetRequiredService<ILogger<SignInManager<ApplicationUser>>>();
            _schemes = services.BuildServiceProvider().GetRequiredService<IAuthenticationSchemeProvider>();
            _userClaimsPrincipalFactory = services.BuildServiceProvider().GetRequiredService<IUserClaimsPrincipalFactory<ApplicationUser>>();

            _userConfirmation = new DefaultUserConfirmation<ApplicationUser>();

            // Create SignInManager
            _signInManager = new SignInManager<ApplicationUser>(
                _userManager,
                _httpContextAccessor,
                _userClaimsPrincipalFactory,
                _identityOptions,
                _logger,
                _schemes,
                _userConfirmation);
        }

        public class DefaultUserConfirmation<TUser> : IUserConfirmation<TUser> where TUser : class
        {
            public Task<bool> IsConfirmedAsync(UserManager<TUser> manager, TUser user)
            {
                return Task.FromResult(true);
            }
        }
        [Test]
        public async Task CreateTransportCompany_ValidInput_ReturnsRedirectToActionResult()
        {
            var httpContext = new DefaultHttpContext();
            _httpContextAccessor.HttpContext = httpContext;

            httpContext.RequestServices = _serviceProvider;
            // Arrange
            var controller = new TransportCompanyController(_context, _userManager)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal(new ClaimsIdentity()) }
                }
            };

            // Create and sign in a user
            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                FirstName = "John",
                LastName = "Doe"
            };
            await _userManager.CreateAsync(user, "Test@123");
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Update the controller context with the signed-in user
            controller.ControllerContext.HttpContext.User = _httpContextAccessor.HttpContext.User;

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a test file."));
            var viewModel = new TransportCompanyViewModel
            {
                Name = "Test Transport Company",
                Logo = new FormFile(stream, 0, stream.Length, "testLogo.jpg", "image/jpeg")
            };

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public async Task CreateTransportCompany_InvalidInput_ReturnsViewResult()
        {
            // Arrange
            var controller = new TransportCompanyController(_context, _userManager)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal(new ClaimsIdentity()) }
                }
            };
            var viewModel = new TransportCompanyViewModel();

            // Add model state error to simulate invalid input
            controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await controller.Create(viewModel);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up any resources that were used in the test
            _context.Database.EnsureDeleted();
        }
    }
}