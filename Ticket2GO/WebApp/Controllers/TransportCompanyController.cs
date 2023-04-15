using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;
using System.IO;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin,Company Manager")]
    public class TransportCompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransportCompanyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Company Manager"))
            {
                return RedirectToAction("ManagerIndex");
            }
            else if (roles.Contains("Admin"))
            {
                return View(await _context.TransportCompanies.ToListAsync());
            }
            else
            {
                return Forbid();
            }
        }

        [Authorize(Roles = "Company Manager")]
        public async Task<IActionResult> ManagerIndex()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var transportCompanies = _context.TransportCompaniesAspNetUsers
                                              .Include(tc => tc.TransportCompany)
                                              .Where(tc => tc.ApplicationUserId == user.Id)
                                              .Select(tc => tc.TransportCompany);

            return View(await transportCompanies.ToListAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransportCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] logoBytes;

                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.Logo.CopyToAsync(memoryStream);
                    logoBytes = memoryStream.ToArray();
                }

                var transportCompany = new TransportCompany
                {
                    Name = viewModel.Name,
                    Logo = logoBytes
                };

                _context.Add(transportCompany);
                await _context.SaveChangesAsync();

                var user = await _userManager.GetUserAsync(HttpContext.User);
                var transportCompanyAspNetUser = new TransportCompanyAspNetUser
                {
                    TransportCompanyId = transportCompany.TransportCompanyId,
                    ApplicationUserId = user.Id
                };

                _context.TransportCompaniesAspNetUsers.Add(transportCompanyAspNetUser);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }


        [Authorize(Roles = "Company Manager")]
        public async Task<IActionResult> MyTransportCompany()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userTransportCompany = _context.TransportCompaniesAspNetUsers
                .FirstOrDefault(tc => tc.ApplicationUserId == currentUser.Id);

            if (userTransportCompany == null)
            {
                return NotFound();
            }

            return View("Details", userTransportCompany.TransportCompany);
        }

        [HttpGet]
        public async Task<IActionResult> Buses(Guid transportCompanyId)
        {
            var transportCompany = await _context.TransportCompanies.FindAsync(transportCompanyId);
            if (transportCompany == null)
            {
                return NotFound();
            }

            var buses = _context.Buses.Where(b => b.TransportCompanyId == transportCompanyId);
            ViewData["TransportCompanyName"] = transportCompany.Name;
            ViewData["TransportCompanyId"] = transportCompanyId;

            return View(await buses.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateBus(Guid transportCompanyId)
        {
            ViewData["TransportCompanyId"] = transportCompanyId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBus(Guid transportCompanyId, CreateBusViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var bus = new Bus
                {
                    Name = viewModel.Name,
                    SeatsNumber = viewModel.SeatsNumber,
                    TransportCompanyId = transportCompanyId
                };
                _context.Add(bus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Buses), new { transportCompanyId });
            }

            ViewData["TransportCompanyId"] = transportCompanyId;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var transportCompany = await _context.TransportCompanies.FindAsync(id);
            if (transportCompany == null)
            {
                return NotFound();
            }

            var transportCompaniesAspNetUsers = _context.TransportCompaniesAspNetUsers
                .Where(tc => tc.TransportCompanyId == id);
            _context.TransportCompaniesAspNetUsers.RemoveRange(transportCompaniesAspNetUsers);

            _context.TransportCompanies.Remove(transportCompany);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
