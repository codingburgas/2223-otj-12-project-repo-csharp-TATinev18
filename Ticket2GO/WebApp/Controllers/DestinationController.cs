using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Identity.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Company Manager, Admin")]
    public class DestinationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDestinationService _destinationService;

        public DestinationController(UserManager<ApplicationUser> userManager, IDestinationService destinationService)
        {
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
            return View(await _destinationService.GetCompaniesAsync(await _userManager.GetUserAsync(User)));
        }

        [HttpPost]
        public async Task<IActionResult> RedirectToCreateWithCompanyId(CreateDestinationViewModel viewModel)
        {
            if (viewModel.SelectedCompanyId != Guid.Empty)
            {
                return RedirectToAction("Create", new { companyId = viewModel.SelectedCompanyId });
            }
            else
            {
                ModelState.AddModelError("SelectedCompanyId", "Изберете компания.");
                return View("SelectCompany", await _destinationService.GetCompaniesAsync(await _userManager.GetUserAsync(User)));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                return RedirectToAction("SelectCompany");
            }

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

            if (!viewModel.SelectedBusId.HasValue)
            {
                ModelState.AddModelError("SelectedBusId", "Изберете автобус");
            }

            string companyId = TempData["CompanyId"]?.ToString() ?? string.Empty;
            TempData["CompanyId"] = companyId;

            ViewData["BusId"] = _destinationService.GetBusesSelectList();
            ViewData["CompanyId"] = _destinationService.GetCompaniesSelectList();
            _destinationService.GetBuses(viewModel, companyId);

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
