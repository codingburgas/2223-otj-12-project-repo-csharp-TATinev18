using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private SelectList GetAvailableDestination()
        {
            var availableDestinations = _context.Destinations.Include(d => d.Bus).ThenInclude(b => b.TransportCompany)
                .Select(d => new SelectListItem
                {
                    Value = d.DestinationId.ToString(),
                    Text = $"{d.StartingDestination} - {d.FinalDestination} ({d.Bus.TransportCompany.Name})"
                });

            return new SelectList(availableDestinations, "Value", "Text");
        }

        [HttpGet]
        public async Task<IActionResult> BookTicket(string transportCompanyId, string startingDestination, string finalDestination, DateTime? date)
        {
            IQueryable<Destination> destinations = _context.Destinations.Include(d => d.Bus).ThenInclude(b => b.TransportCompany);

            if (!string.IsNullOrEmpty(startingDestination))
            {
                destinations = destinations.Where(d => d.StartingDestination.Contains(startingDestination));
            }

            if (!string.IsNullOrEmpty(finalDestination))
            {
                destinations = destinations.Where(d => d.FinalDestination.Contains(finalDestination));
            }

            if (!string.IsNullOrEmpty(transportCompanyId))
            {
                Guid parsedTransportCompanyId = Guid.Parse(transportCompanyId);
                destinations = destinations.Where(d => d.Bus.TransportCompanyId == parsedTransportCompanyId);
            }

            if (date.HasValue)
            {
                destinations = destinations.Where(d => d.Departure.Date == date.Value.Date);
            }

            destinations = destinations.Where(d => d.Departure >= DateTime.Now);

            var filteredDestinations = await destinations.ToListAsync();

            var model = new BookTicketViewModel
            {
                Destinations = filteredDestinations,
                Companies = new SelectList(_context.TransportCompanies, "TransportCompanyId", "Name"),
                SelectedCompanyId = string.IsNullOrEmpty(transportCompanyId) ? Guid.Empty : Guid.Parse(transportCompanyId),
                SelectedStartingDestination = startingDestination,
                SelectedFinalDestination = finalDestination,
                SelectedDate = date ?? DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket(BookTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var originDestination = _context.Destinations
                    .FirstOrDefault(d => d.DestinationId == model.SelectedOriginDestinationId);

                var ticket = new Ticket
                {
                    ApplicationUserId = _userManager.GetUserId(User),
                    TotalPrice = originDestination.Price,
                    SeatNumber = model.SelectedSeat
                };

                if (model.IsRoundTrip)
                {
                    var returnDestination = GetReturnDestinations(model.SelectedOriginDestinationId, ds => ds.FirstOrDefault());
                    ticket.TotalPrice += returnDestination.Price;
                }

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                var ticketDestination = new TicketDestination
                {
                    TicketId = ticket.TicketId,
                    DestinationId = model.SelectedOriginDestinationId
                };

                _context.TicketsDestinations.Add(ticketDestination);

                if (model.IsRoundTrip)
                {
                    var returnDestination = GetReturnDestinations(model.SelectedOriginDestinationId, ds => ds.FirstOrDefault());

                    var returnTicketDestination = new TicketDestination
                    {
                        TicketId = ticket.TicketId,
                        DestinationId = returnDestination.DestinationId
                    };

                    _context.TicketsDestinations.Add(returnTicketDestination);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation");
            }

            model.AvailableDestinations = GetAvailableDestination();
            model.MaxSeats = GetMaxSeats(model.SelectedOriginDestinationId);
            model.ReturnDestinations = GetReturnDestinations(model.SelectedOriginDestinationId, ds => ds.Select(d => new SelectListItem
            {
                Value = d.DestinationId.ToString(),
                Text = $"{d.StartingDestination} - {d.FinalDestination} ({d.Bus.TransportCompany.Name}) - {d.Departure.ToString("dd/MM/yyyy HH:mm")} - {d.TimeOfArrival.ToString("dd/MM/yyyy hh:mm")} - {d.Price.ToString("C2")}"
            }).ToList());

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SelectSeat(Guid id)
        {
            var destination = await _context.Destinations
                .Include(d => d.Bus)
                .ThenInclude(b => b.TransportCompany)
                .FirstOrDefaultAsync(d => d.DestinationId == id);

            if (destination == null)
            {
                return NotFound();
            }

            var returnDestinations = GetReturnDestinations(id, ds => ds.Select(d => new SelectListItem
            {
                Value = d.DestinationId.ToString(),
                Text = $"{d.StartingDestination} - {d.FinalDestination} ({d.Bus.TransportCompany.Name}) - {d.Departure.ToString("dd/MM/yyyy HH:mm")} - {d.TimeOfArrival.ToString("dd/MM/yyyy hh:mm")} - {d.Price.ToString("C2")}"
            }).ToList());

            var availableSeats = await GetAvailableSeatsAsync(destination.BusId, destination.Bus.SeatsNumber);

            var model = new SelectSeatViewModel
            {
                DestinationId = destination.DestinationId,
                StartingDestination = destination.StartingDestination,
                FinalDestination = destination.FinalDestination,
                Departure = destination.Departure,
                TimeOfArrival = destination.TimeOfArrival,
                Price = destination.Price,
                BusName = destination.Bus.Name,
                TransportCompany = destination.Bus.TransportCompany.Name,
                MaxSeats = destination.Bus.SeatsNumber,
                ReturnDestinations = returnDestinations,
                AvailableSeats = availableSeats
            };

            if (TempData["ErrorMessage"] != null)
            {
                ModelState.AddModelError("SelectedSeat", TempData["ErrorMessage"].ToString());
            }

            return View(model);
        }

        private async Task<IEnumerable<int>> GetAvailableSeatsAsync(Guid busId, int seatsNumber)
        {
            var destinations = await _context.Destinations
                .Where(d => d.BusId == busId)
                .Select(d => d.DestinationId)
                .ToListAsync();

            var takenSeats = await _context.TicketsDestinations
                .Where(td => destinations.Contains(td.DestinationId))
                .Join(_context.Tickets, td => td.TicketId, t => t.TicketId, (td, t) => t.SeatNumber)
                .Distinct()
                .ToListAsync();

            var availableSeats = Enumerable.Range(1, seatsNumber).Where(seat => !takenSeats.Contains(seat)).ToList();

            return availableSeats ?? new List<int>();
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(SelectSeatViewModel model)
        {
            if (!ModelState.IsValid && ModelState[nameof(model.SelectedSeat)].Errors.Any())
            {
                TempData["ErrorMessage"] = "Please select a seat.";
                return RedirectToAction("SelectSeat", new { id = model.DestinationId });
            }

            decimal totalPrice = model.Price + _context.Destinations.Where(d => d.DestinationId == model.SelectedReturnDestinationId).First().Price;
            if (!string.IsNullOrEmpty(model.ReturnDestinationId))
            {
                var returnDestination = await _context.Destinations
                    .FirstOrDefaultAsync(d => d.DestinationId == Guid.Parse(model.ReturnDestinationId));
                if (returnDestination != null)
                {
                    totalPrice += returnDestination.Price;
                }
            }
            var ticket = new Ticket
            {
                ApplicationUserId = _userManager.GetUserId(User),
                TotalPrice = totalPrice,
                SeatNumber = model.SelectedSeat.Value
            };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            var ticketDestination = new TicketDestination
            {
                TicketId = ticket.TicketId,
                DestinationId = model.DestinationId
            };
            _context.TicketsDestinations.Add(ticketDestination);
            await _context.SaveChangesAsync();

            if (model.SelectedReturnDestinationId.HasValue)
            {
                var ticketDestination2 = new TicketDestination
                {
                    TicketId = ticket.TicketId,
                    DestinationId = model.SelectedReturnDestinationId.Value
                };
                _context.TicketsDestinations.Add(ticketDestination2);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Confirmation");
        }

        private int GetMaxSeats(Guid destinationId)
        {
            if (destinationId == Guid.Empty) return 0;

            var bus = _context.Destinations
                .Include(d => d.Bus)
                .FirstOrDefault(d => d.DestinationId == destinationId)?.Bus;

            return bus?.SeatsNumber ?? 0;
        }

        private T GetReturnDestinations<T>(Guid originDestinationId, Func<IEnumerable<Destination>, T> selector)
        {
            var originDestination = _context.Destinations
                .FirstOrDefault(d => d.DestinationId == originDestinationId);

            if (originDestination == null)
                return default(T);

            var returnDestinations = _context.Destinations.Include(d => d.Bus).ThenInclude(b => b.TransportCompany)
                .Where(d => d.StartingDestination == originDestination.FinalDestination &&
                            d.FinalDestination == originDestination.StartingDestination &&
                            d.Departure <= originDestination.Departure.AddDays(14) &&
                            d.Departure > originDestination.TimeOfArrival);

            return selector(returnDestinations);
        }
        public async Task<IActionResult> MyTicket()
        {
            var userId = _userManager.GetUserId(User);
            var tickets = await _context.Tickets
                .Where(t => t.ApplicationUserId == userId)
                .Include(t => t.TicketDestinations)
                .ThenInclude(td => td.Destination)
                .ThenInclude(d => d.Bus)
                .ToListAsync();

            if (tickets == null || tickets.Count == 0)
            {
                ViewData["Message"] = "You currently have no booked tickets.";
            }

            return View(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.TicketDestinations)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId && t.ApplicationUserId == _userManager.GetUserId(User));

            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyTicket");
        }
    }
}