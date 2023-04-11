﻿using System;
using System.Collections.Generic;
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

        private IQueryable<Destination> FilterDestinations(string transportCompanyId, string startingDestination, string finalDestination, DateTime? date)
        {
            var query = _context.Destinations.Include(d => d.Bus).ThenInclude(b => b.TransportCompany).AsQueryable();

            if (!string.IsNullOrEmpty(transportCompanyId))
            {
                var companyIdGuid = Guid.Parse(transportCompanyId);
                query = query.Where(d => d.Bus.TransportCompanyId == companyIdGuid);
            }


            if (!string.IsNullOrEmpty(startingDestination))
            {
                query = query.Where(d => d.StartingDestination.Contains(startingDestination));
            }

            if (!string.IsNullOrEmpty(finalDestination))
            {
                query = query.Where(d => d.FinalDestination.Contains(finalDestination));
            }

            if (date.HasValue)
            {
                query = query.Where(d => d.Departure.Date == date.Value.Date);
            }

            return query;
        }

        // Modify the BookTicket GET action
        [HttpGet]
        public async Task<IActionResult> BookTicket(string transportCompanyId, string startingDestination, string finalDestination, DateTime? date)
        {
            var destinations = await FilterDestinations(transportCompanyId, startingDestination, finalDestination, date).ToListAsync();
            var model = new BookTicketViewModel
            {
                Destinations = destinations ?? new List<Destination>(),
                Companies = new SelectList(_context.TransportCompanies, "TransportCompanyId", "Name"),
                SelectedCompanyId = string.IsNullOrEmpty(transportCompanyId) ? Guid.Empty : Guid.Parse(transportCompanyId),
                SelectedStartingDestination = startingDestination,
                SelectedFinalDestination = finalDestination,
                SelectedDate = date ?? DateTime.Now
            };

            return View(model);
        }



        // Add a new action to select seats and show the price
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
                MaxSeats = destination.Bus.SeatsNumber
            };

            return View(model);
        }

        // Add a new action to confirm the booking
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(SelectSeatViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ticket = new Ticket
                {
                    ApplicationUserId = _userManager.GetUserId(User),
                    TotalPrice = model.Price
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                var ticketDestination = new TicketDestination
                {
                    TicketId = ticket.TicketId,
                    DestinationId = model.DestinationId,
                    SeatNumber = model.SelectedSeat
                };

                _context.TicketsDestinations.Add(ticketDestination);
                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation");
            }

            return View("SelectSeat", model);
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
                    TotalPrice = originDestination.Price
                };

                if (model.IsRoundTrip)
                {
                    var returnDestination = GetReturnDestination(model.SelectedOriginDestinationId);
                    ticket.TotalPrice += returnDestination.Price;
                }

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                var ticketDestination = new TicketDestination
                {
                    TicketId = ticket.TicketId,
                    DestinationId = model.SelectedOriginDestinationId,
                    SeatNumber = model.SelectedSeat
                };

                _context.TicketsDestinations.Add(ticketDestination);

                if (model.IsRoundTrip)
                {
                    var returnDestination = GetReturnDestination(model.SelectedOriginDestinationId);

                    var returnTicketDestination = new TicketDestination
                    {
                        TicketId = ticket.TicketId,
                        DestinationId = returnDestination.DestinationId,
                        SeatNumber = model.SelectedReturnSeat.HasValue ? model.SelectedReturnSeat.Value : (int?)null
                    };

                    _context.TicketsDestinations.Add(returnTicketDestination);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation");
            }

            model.AvailableDestinations = GetAvailableDestination();
            model.MaxSeats = GetMaxSeats(model.SelectedOriginDestinationId);
            model.ReturnDestinations = GetReturnDestinations(model.SelectedOriginDestinationId);

            return View(model);
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        private Destination GetReturnDestination(Guid originDestinationId)
        {
            var originDestination = _context.Destinations
                .FirstOrDefault(d => d.DestinationId == originDestinationId);

            if (originDestination == null)
                return null;

            var returnDestination = _context.Destinations
                .Where(d => d.StartingDestination == originDestination.FinalDestination &&
                            d.FinalDestination == originDestination.StartingDestination &&
                            d.Departure <= originDestination.Departure.AddDays(14))
                .FirstOrDefault();

            return returnDestination;
        }
        private int GetMaxSeats(Guid destinationId)
        {
            if (destinationId == Guid.Empty) return 0;

            var bus = _context.Destinations
                .Include(d => d.Bus)
                .FirstOrDefault(d => d.DestinationId == destinationId)?.Bus;

            return bus?.SeatsNumber ?? 0;
        }

        private SelectList GetReturnDestinations(Guid originDestinationId)
        {
            var originDestination = _context.Destinations
                .FirstOrDefault(d => d.DestinationId == originDestinationId);

            if (originDestination == null)
                return new SelectList(Enumerable.Empty<SelectListItem>());

            var returnDestinations = _context.Destinations.Include(d => d.Bus).ThenInclude(b => b.TransportCompany)
                .Where(d => d.StartingDestination == originDestination.FinalDestination &&
                            d.FinalDestination == originDestination.StartingDestination &&
                            d.Departure <= originDestination.Departure.AddDays(14))
                .Select(d => new SelectListItem
                {
                    Value = d.DestinationId.ToString(),
                    Text = $"{d.StartingDestination} - {d.FinalDestination} ({d.Bus.TransportCompany.Name})"
                });

            return new SelectList(returnDestinations, "Value", "Text");
        }
    }
}