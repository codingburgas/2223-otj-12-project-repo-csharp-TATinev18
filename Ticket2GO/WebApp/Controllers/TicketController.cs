using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class TicketController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITicketService _ticketService;

        public TicketController(UserManager<ApplicationUser> userManager, ITicketService ticketService)
        {
            _userManager = userManager;
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> BookTicket(string transportCompanyId, string startingDestination, string finalDestination, DateTime? date)
        {
            return View(await _ticketService.GetTickets(transportCompanyId, startingDestination, finalDestination, date));
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket(BookTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.BookTicket(model, _userManager.GetUserId(User));

                return RedirectToAction("Confirmation");
            }

            _ticketService.GetReturnDestinations(model);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SelectSeat(Guid id)
        {
            Destination? destination = await _ticketService.GetDestinations(id);

            if (destination == null)
            {
                return NotFound();
            }

            if (TempData["ErrorMessage"] != null)
            {
                ModelState.AddModelError("SelectedSeat", TempData["ErrorMessage"]?.ToString() ?? "");
            }

            return View(await _ticketService.GenerateSelectSeatViewModel(id, destination));
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(SelectSeatViewModel model)
        {
            if (!ModelState.IsValid && ModelState[nameof(model.SelectedSeat)]?.Errors?.Any() == true)
            {
                TempData["ErrorMessage"] = "Моля, изберете място.";
                return RedirectToAction("SelectSeat", new { id = model.DestinationId });
            }

            await _ticketService.ConfirmTicket(model, _userManager.GetUserId(User));

            return RedirectToAction("Confirmation");
        }

        public async Task<IActionResult> MyTicket()
        {
            List<Ticket>? tickets = await _ticketService.GetTickets(_userManager.GetUserId(User));

            if (tickets == null || tickets.Count == 0)
            {
                ViewData["Message"] = "В момента нямате запазени билети.";
            }

            return View(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            if(!await _ticketService.Cancel(ticketId, _userManager.GetUserId(User)))
            {
                NotFound();
            }

            return RedirectToAction("MyTicket");
        }
    }
}