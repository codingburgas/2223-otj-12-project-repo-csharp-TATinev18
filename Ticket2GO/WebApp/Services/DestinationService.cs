using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly ApplicationDbContext _context;

        public DestinationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Destination>> GetDestinations()
        {
            return await _context.Destinations
                                             .Include(d => d.Bus)
                                             .ThenInclude(b => b.TransportCompany)
                                             .ToListAsync();
        }

        public CreateDestinationViewModel GetCompanies(ApplicationUser user)
        {
            string managerId = user?.Id;
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
            return viewModel;
        }

        public async Task<CreateDestinationViewModel> CreateDestination(Guid companyId)
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
            return viewModel;
        }

        public async Task<Destination?> GetDestination(Guid? id)
        {
            return await _context.Destinations
                            .Include(d => d.Bus)
                            .FirstOrDefaultAsync(m => m.DestinationId == id);
        }

        public async Task DeleteDestination(bool deleteAllRepetitions, Destination? destination)
        {
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
        }

        public async Task CreateDestination(CreateDestinationViewModel viewModel)
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
        }

        public void GetBuses(CreateDestinationViewModel viewModel, string companyId)
        {
            viewModel.Buses = _context.Buses
                .Where(b => b.TransportCompanyId == new Guid(companyId))
                .Select(b => new SelectListItem
                {
                    Value = b.BusId.ToString(),
                    Text = b.Name
                }).ToList();
        }
    }
}
