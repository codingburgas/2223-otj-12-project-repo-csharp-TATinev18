﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        public TicketService(ApplicationDbContext context)
        {
            _context = context; ;
        }
        public async Task<BookTicketViewModel> GetTickets(string transportCompanyId, string startingDestination, string finalDestination, DateTime? date)
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

            filteredDestinations = filteredDestinations.Where(d => (GetAvailableSeatsAsync(d)).Result.Any()).ToList();

            var model = new BookTicketViewModel
            {
                Destinations = filteredDestinations,
                Companies = new SelectList(_context.TransportCompanies, "TransportCompanyId", "Name"),
                SelectedCompanyId = string.IsNullOrEmpty(transportCompanyId) ? Guid.Empty : Guid.Parse(transportCompanyId),
                SelectedStartingDestination = startingDestination,
                SelectedFinalDestination = finalDestination,
                SelectedDate = date ?? DateTime.Now
            };
            return model;
        }


        public async Task BookTicket(BookTicketViewModel model, string userId)
        {
            var originDestination = _context.Destinations
                                .FirstOrDefault(d => d.DestinationId == model.SelectedOriginDestinationId);

            if (originDestination == null)
            {
                throw new ArgumentException("Невалидна първоначална дестинация.");
            }

            var ticket = new Ticket
            {
                ApplicationUserId = userId,
                TotalPrice = originDestination.Price,
                SeatNumber = model.SelectedSeat
            };

            if (model.IsRoundTrip)
            {
                var returnDestination = GetReturnDestinations(model.SelectedOriginDestinationId, ds => ds.FirstOrDefault());

                if (returnDestination == null)
                {
                    throw new ArgumentException("Invalid return destination.");
                }

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

                if (returnDestination == null)
                {
                    throw new ArgumentException("Невалидна дестинация за връщане.");
                }

                var returnTicketDestination = new TicketDestination
                {
                    TicketId = ticket.TicketId,
                    DestinationId = returnDestination.DestinationId
                };

                _context.TicketsDestinations.Add(returnTicketDestination);
            }

            await _context.SaveChangesAsync();
        }


        public void GetReturnDestinations(BookTicketViewModel model)
        {
            model.AvailableDestinations = GetAvailableDestination();
            model.MaxSeats = GetMaxSeats(model.SelectedOriginDestinationId);
            model.ReturnDestinations = GetReturnDestinations(model.SelectedOriginDestinationId, ds => ds.Select(d => new SelectListItem
            {
                Value = d.DestinationId.ToString(),
                Text = $"{d.StartingDestination} - {d.FinalDestination} ({d.Bus.TransportCompany.Name}) - {d.Departure.ToString("dd/MM/yyyy HH:mm")} - {d.TimeOfArrival.ToString("dd/MM/yyyy hh:mm")} - {d.Price.ToString("C2")}"
            }).ToList());
        }

        public async Task<SelectSeatViewModel> GenerateSelectSeatViewModel(Guid id, Destination? destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var returnDestinations = GetReturnDestinations(id, ds => ds.Select(d => new SelectListItem
            {
                Value = d.DestinationId.ToString(),
                Text = $"{d.StartingDestination} - {d.FinalDestination} ({d.Bus.TransportCompany.Name}) - {d.Departure.ToString("dd/MM/yyyy HH:mm")} - {d.TimeOfArrival.ToString("dd/MM/yyyy hh:mm")} - {d.Price.ToString("C2")}"
            }).ToList());

            var availableSeats = await GetAvailableSeatsAsync(destination);

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
            return model;
        }

        public async Task<Destination?> GetDestinations(Guid id)
        {
            return await _context.Destinations
                            .Include(d => d.Bus)
                            .ThenInclude(b => b.TransportCompany)
                            .FirstOrDefaultAsync(d => d.DestinationId == id);
        }

        public async Task ConfirmTicket(SelectSeatViewModel model, string userId)
        {
            var destination = await _context.Destinations
                .Include(d => d.Bus)
                .FirstOrDefaultAsync(d => d.DestinationId == model.DestinationId);

            if (destination == null || destination.Bus == null)
            {
                throw new Exception("Автобус или маршрут не могат да бъдат заредени.");
            }

            Destination? returnDestination = null;

            if (model.SelectedReturnDestinationId.HasValue)
            {
                returnDestination = await _context.Destinations
                    .Include(d => d.Bus)
                    .FirstOrDefaultAsync(d => d.DestinationId == model.SelectedReturnDestinationId.Value);

                if (returnDestination == null || returnDestination.Bus == null)
                {
                    throw new Exception("Дестинация за обратна посока или автобус не могат да бъдат заредени.");
                }
            }

            decimal totalPrice = model.Price + (returnDestination?.Price ?? 0);

            if (!model.SelectedSeat.HasValue)
            {
                throw new ArgumentException("Номер на място не е избран.");
            }

            var ticket = new Ticket
            {
                ApplicationUserId = userId,
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
                if (returnDestination == null)
                {
                    throw new Exception("Няма опция за връщане.");
                }

                var availableSeatsForReturnDestination = await GetAvailableSeatsAsync(returnDestination);

                if (!availableSeatsForReturnDestination.Contains(model.SelectedSeat.Value))
                {
                    model.SelectedSeat = availableSeatsForReturnDestination.FirstOrDefault();

                    if (!model.SelectedSeat.HasValue)
                    {
                        throw new ArgumentException("Няма налични места за тази дестинация.");
                    }

                    ticket.SeatNumber = model.SelectedSeat.Value;

                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }

                var ticketDestination2 = new TicketDestination
                {
                    TicketId = ticket.TicketId,
                    DestinationId = model.SelectedReturnDestinationId.Value
                };

                _context.TicketsDestinations.Add(ticketDestination2);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Ticket>?> GetTickets(string userId)
        {
            return await _context.Tickets
                            .Where(t => t.ApplicationUserId == userId)
                            .Include(t => t.TicketDestinations)
                            .ThenInclude(td => td.Destination)
                            .ThenInclude(d => d.Bus)
                            .ToListAsync();
        }
        public async Task<bool> Cancel(Guid ticketId, string userId)
        {
            var ticket = await _context.Tickets
                            .Include(t => t.TicketDestinations)
                            .FirstOrDefaultAsync(t => t.TicketId == ticketId && t.ApplicationUserId == userId);

            if (ticket == null)
            {
                return false;
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<IEnumerable<int>> GetAvailableSeatsAsync(Destination destination)
        {
            if (destination == null || destination.Bus == null)
            {
                throw new ArgumentNullException(nameof(destination), "Дестинацията или автобуса не могат да бъдат без стойност.");
            }

            var destinations = await _context.Destinations
                .Where(d => d.BusId == destination.BusId && d.Departure.Date == destination.Departure.Date)
                .Select(d => d.DestinationId)
                .ToListAsync();

            var takenSeats = await _context.TicketsDestinations
                .Where(td => destinations.Contains(td.DestinationId))
                .Join(_context.Tickets, td => td.TicketId, t => t.TicketId, (td, t) => t.SeatNumber)
                .Distinct()
                .ToListAsync();

            var availableSeats = Enumerable.Range(1, destination.Bus.SeatsNumber).Where(seat => !takenSeats.Contains(seat)).ToList();

            return availableSeats;
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
                return default!;

            var returnDestinations = _context.Destinations.Include(d => d.Bus).ThenInclude(b => b.TransportCompany)
                .Where(d => d.StartingDestination == originDestination.FinalDestination &&
                            d.FinalDestination == originDestination.StartingDestination &&
                            d.Departure <= originDestination.Departure.AddDays(14) &&
                            d.Departure > originDestination.TimeOfArrival)
                .ToList();

            var destinationsWithSeatsAvailable = new List<Destination>();

            foreach (var destination in returnDestinations)
            {
                var availableSeats = GetAvailableSeatsAsync(destination).Result;

                if (availableSeats.Any())
                {
                    destinationsWithSeatsAvailable.Add(destination);
                }
            }

            return selector(destinationsWithSeatsAvailable)!;
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
    }
}
