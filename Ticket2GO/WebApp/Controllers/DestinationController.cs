using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class DestinationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDestinationService _destinationService;

        public DestinationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDestinationService destinationService)
        {
            _context = context;
            _userManager = userManager;
            _destinationService = destinationService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _destinationService.GetDestinations());
        }

        [HttpGet]
        public async Task<IActionResult> SelectCompany()
        {
            return View(_destinationService.GetCompanies(await _userManager.GetUserAsync(User)));
        }

        [HttpPost]
        public IActionResult RedirectToCreateWithCompanyId(CreateDestinationViewModel viewModel)
        {
            return RedirectToAction("Create", new { companyId = viewModel.SelectedCompanyId });
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid companyId)
        {
            TempData["CompanyId"] = companyId.ToString();
            return View(await _destinationService.CreateDestination(companyId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDestinationViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.SelectedBusId.HasValue)
            {
                await _destinationService.CreateDestination(viewModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("SelectedBusId", "Bus is required");
            }

            ViewData["BusId"] = new SelectList(_context.Buses, "BusId", "Name", viewModel.SelectedBusId);
            ViewData["CompanyId"] = new SelectList(_context.TransportCompanies, "CompanyId", "Name", viewModel.SelectedCompanyId);
            _destinationService.GetBuses(viewModel, TempData["CompanyId"].ToString());

            return View(viewModel);
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Destination? destination = await _destinationService.GetDestination(id);

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

            Destination? destination = await _destinationService.GetDestination(id);

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
            Destination? destination = await _destinationService.GetDestination(id);

            if (destination == null)
            {
                return NotFound();
            }

            await _destinationService.DeleteDestination(deleteAllRepetitions, destination);
            return RedirectToAction(nameof(Index));
        }
    }
}
