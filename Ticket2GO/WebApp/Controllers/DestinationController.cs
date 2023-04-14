using System.ComponentModel.Design;
using System.Drawing;
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
            TempData["CompanyId"] = companyId.ToString();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDestinationViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.SelectedBusId.HasValue)
            {
                var destinations = new List<Destination>();

                if (viewModel.RepeatingDayOfWeek.HasValue && viewModel.NumberOfRepetitions.HasValue)
                {
                    var currentDate = viewModel.Departure;
                    int repetitions = 0;

                    while (repetitions < viewModel.NumberOfRepetitions.Value)
                    {
                        if (currentDate.DayOfWeek == viewModel.RepeatingDayOfWeek.Value)
                        {
                            destinations.Add(new Destination
                            {
                                StartingDestination = viewModel.StartingDestination,
                                FinalDestination = viewModel.FinalDestination,
                                Duration = viewModel.Duration,
                                Departure = currentDate,
                                TimeOfArrival = currentDate.Add(viewModel.Duration),
                                BusId = viewModel.SelectedBusId.Value,
                                RepeatingDayOfWeek = viewModel.RepeatingDayOfWeek,
                                Price = viewModel.TotalPrice
                            });

                            repetitions++;
                        }

                        currentDate = currentDate.AddDays(1);
                    }
                }
                else
                {
                    destinations.Add(new Destination
                    {
                        StartingDestination = viewModel.StartingDestination,
                        FinalDestination = viewModel.FinalDestination,
                        Duration = viewModel.Duration,
                        Departure = viewModel.Departure,
                        TimeOfArrival = viewModel.TimeOfArrival,
                        BusId = viewModel.SelectedBusId.Value,
                        RepeatingDayOfWeek = viewModel.RepeatingDayOfWeek,
                        Price = viewModel.TotalPrice
                    });
                }

                _context.AddRange(destinations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("SelectedBusId", "Bus is required");
            }

            ViewData["BusId"] = new SelectList(_context.Buses, "BusId", "Name", viewModel.SelectedBusId);
            ViewData["CompanyId"] = new SelectList(_context.TransportCompanies, "CompanyId", "Name", viewModel.SelectedCompanyId);
            viewModel.Buses = _context.Buses
                .Where(b => b.TransportCompanyId == new Guid(TempData["CompanyId"].ToString()))
                .Select(b => new SelectListItem
                {
                    Value = b.BusId.ToString(),
                    Text = b.Name
                }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .Include(d => d.Bus)
                .FirstOrDefaultAsync(m => m.DestinationId == id);

            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .Include(d => d.Bus)
                .FirstOrDefaultAsync(m => m.DestinationId == id);

            if (destination == null)
            {
                return NotFound();
            }

            ViewBag.DeleteAllRepetitions = false;

            return View(destination);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, bool deleteAllRepetitions = false)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }

            _context.Destinations.Remove(destination);

            if (deleteAllRepetitions)
            {
                var repeatedDestinations = _context.Destinations
                    .Where(d => d.StartingDestination == destination.StartingDestination &&
                                d.FinalDestination == destination.FinalDestination &&
                                d.BusId == destination.BusId &&
                                d.Duration == destination.Duration &&
                                d.RepeatingDayOfWeek == destination.RepeatingDayOfWeek)
                    .ToList();

                foreach (var repeatedDestination in repeatedDestinations)
                {
                    int daysDifference = (repeatedDestination.Departure - destination.Departure).Days;
                    if (daysDifference % 7 == 0)
                    {
                        _context.Destinations.Remove(repeatedDestination);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
