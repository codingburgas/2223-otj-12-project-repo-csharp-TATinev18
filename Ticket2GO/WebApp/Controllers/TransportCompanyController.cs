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
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin,Company Manager")]
    public class TransportCompanyController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransportCompanyService _transportCompanyService;

        public TransportCompanyController(UserManager<ApplicationUser> userManager, ITransportCompanyService transportCompanyService)
        {
            _userManager = userManager;
            _transportCompanyService = transportCompanyService;
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
                return View(await _transportCompanyService.GetAllTransportCompanies());
            }
            else
            {
                return Forbid();
            }
        }

        [Authorize(Roles = "Company Manager")]
        public async Task<IActionResult> ManagerIndex()
        {
            return View(await _transportCompanyService.GetTransportCompanies(await _userManager.GetUserAsync(HttpContext.User)));
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportCompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _transportCompanyService.CreateTransportCompany(viewModel, await _userManager.GetUserAsync(HttpContext.User));

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Company Manager")]
        public async Task<IActionResult> MyTransportCompany()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            TransportCompanyAspNetUser? userTransportCompany = _transportCompanyService.GetCurrentUsersTransportCompany(currentUser);

            if (userTransportCompany == null)
            {
                return NotFound();
            }

            return View("Details", userTransportCompany.TransportCompany);
        }



        [HttpGet]
        public async Task<IActionResult> Buses(Guid transportCompanyId)
        {
            TransportCompany? transportCompany = await _transportCompanyService.GetTransportCompany(transportCompanyId);
            if (transportCompany == null)
            {
                return NotFound();
            }

            IQueryable<Bus> buses = _transportCompanyService.GetCompanyBuses(transportCompanyId);
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
                await _transportCompanyService.CreateBus(transportCompanyId, viewModel);
                return RedirectToAction(nameof(Buses), new { transportCompanyId });
            }

            ViewData["TransportCompanyId"] = transportCompanyId;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            TransportCompany? transportCompany = await _transportCompanyService.GetTransportCompany(id);
            if (transportCompany == null)
            {
                return NotFound();
            }

            List<Destination> destinations = await _transportCompanyService.GetDestinations(id);

            if (destinations.Any())
            {
                return BadRequest("Транспортната компания не може да бъде изтрита, тъй като има свързани дестинации. Първо изтрийте дестинациите, за да извършите успешно операцията.");
            }

            await _transportCompanyService.DeleteTransportCompany(id, transportCompany);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            TransportCompany? transportCompany = await _transportCompanyService.GetTransportCompany(id);
            if (transportCompany == null)
            {
                return NotFound();
            }

            TransportCompanyViewModel viewModel = _transportCompanyService.GenerateTransportCompnayViewModel(transportCompany);

            return View(viewModel);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TransportCompanyViewModel viewModel)
        {
            if (id != viewModel.TransportCompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TransportCompany? transportCompany = await _transportCompanyService.GetTransportCompany(id);

                    if (transportCompany == null)
                    {
                        return NotFound();
                    }

                    await _transportCompanyService.EditTransportCompany(viewModel, transportCompany);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_transportCompanyService.TransportCompanyExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            TransportCompany? transportCompany = await _transportCompanyService.GetTranspoerCompanyDeatils(id);
            if (transportCompany == null)
            {
                return NotFound();
            }

            return View(transportCompany);
        }
    }
}
