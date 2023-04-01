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
    [Authorize(Roles = "Company Manager")]
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
            var transportCompanies = await _context.TransportCompanies.ToListAsync();
            return View(transportCompanies);
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
                using var memoryStream = new MemoryStream();
                await viewModel.Logo.CopyToAsync(memoryStream);
                byte[] logoBytes = memoryStream.ToArray();

                var transportCompany = new TransportCompany
                {
                    Name = viewModel.Name,
                    Logo = logoBytes
                };

                _context.Add(transportCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }
    }
}
