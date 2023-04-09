using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class DestinationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DestinationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Destination
        public async Task<IActionResult> Index()
        {
            var destinations = await _context.Destinations
                                             .Include(d => d.Bus)
                                             .ThenInclude(b => b.TransportCompany)
                                             .ToListAsync();
            return View(destinations);
        }

        private async Task<string> GetManagerId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }

        [HttpGet]
        public async Task<IActionResult> SelectCompany()
        {
            string managerId = await GetManagerId();
            var companies = _context.TransportCompaniesAspNetUsers
                .Where(t => t.ApplicationUserId == managerId)
                .Select(t => t.TransportCompany)
                .ToList();

            var companySelectListItems = companies.Select(c => new SelectListItem
            {
                Value = c.TransportCompanyId.ToString(),
                Text = c.Name
            }).ToList();

            var viewModel = new CreateDestinationViewModel
            {
                Companies = companySelectListItems
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult RedirectToCreateWithCompanyId(CreateDestinationViewModel viewModel)
        {
            return RedirectToAction("Create", new { companyId = viewModel.SelectedCompanyId });
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid companyId)
        {
            var buses = await _context.Buses.Where(b => b.TransportCompanyId == companyId).ToListAsync();

            var busSelectListItems = buses.Select(b => new SelectListItem
            {
                Value = b.BusId.ToString(),
                Text = b.Name
            }).ToList();

            var viewModel = new CreateDestinationViewModel
            {
                SelectedCompanyId = companyId,
                Buses = busSelectListItems
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDestinationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!viewModel.SelectedBusId.HasValue)
                {
                    ModelState.AddModelError("SelectedBusId", "The Bus field is required.");
                    ViewData["BusId"] = new SelectList(_context.Buses, "BusId", "Name", viewModel.SelectedBusId);
                    ViewData["CompanyId"] = new SelectList(_context.TransportCompanies, "CompanyId", "Name", viewModel.SelectedCompanyId);
                    return View(viewModel);
                }

                var destination = new Destination
                {
                    StartingDestination = viewModel.StartingDestination,
                    FinalDestination = viewModel.FinalDestination,
                    Duration = viewModel.Duration,
                    Departure = viewModel.Departure,
                    TimeOfArrival = viewModel.TimeOfArrival,
                    BusId = viewModel.SelectedBusId.Value
                };
                _context.Add(destination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BusId"] = new SelectList(_context.Buses, "BusId", "Name", viewModel.SelectedBusId);
            ViewData["CompanyId"] = new SelectList(_context.TransportCompanies, "CompanyId", "Name", viewModel.SelectedCompanyId);
            return View(viewModel);
        }
    }
}
