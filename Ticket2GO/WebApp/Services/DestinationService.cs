﻿using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public DestinationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Destination>> GetDestinations()
        {
            return await _context.Destinations
                                 .Include(d => d.Bus)
                                 .ThenInclude(b => b.TransportCompany)
                                 .OrderBy(d => d.Departure)
                                 .ToListAsync();
        }

        public async Task<CreateDestinationViewModel> GetCompaniesAsync(ApplicationUser user)
        {
            var viewModel = new CreateDestinationViewModel();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                viewModel.Companies = _context.TransportCompanies
                    .Select(c => new SelectListItem
                    {
                        Value = c.TransportCompanyId.ToString(),
                        Text = c.Name
                    }).ToList();
            }
            else
            {
                string managerId = user?.Id ?? "";
                var companies = _context.TransportCompaniesAspNetUsers
                    .Where(t => t.ApplicationUserId == managerId)
                    .Select(t => t.TransportCompany)
                    .ToList();

                viewModel.Companies = companies.Select(c => new SelectListItem
                {
                    Value = c.TransportCompanyId.ToString(),
                    Text = c.Name
                }).ToList();
            }

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
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
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
                            BusId = viewModel.SelectedBusId.HasValue ? viewModel.SelectedBusId.Value : default,
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
                if (viewModel.SelectedBusId.HasValue)
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
            }

            _context.AddRange(destinations);
            await _context.SaveChangesAsync();
        }

        public void GetBuses(CreateDestinationViewModel viewModel, string companyId)
        {
            if (Guid.TryParse(companyId, out Guid parsedCompanyId))
            {
                viewModel.Buses = _context.Buses
                    .Where(b => b.TransportCompanyId == parsedCompanyId)
                    .Select(b => new SelectListItem
                    {
                        Value = b.BusId.ToString(),
                        Text = b.Name
                    }).ToList();
            }
            else
            {
                viewModel.Buses = new List<SelectListItem>();
            }
        }

        public SelectList GetBusesSelectList()
        {
            return new SelectList(_context.Buses, "BusId", "Name");
        }

        public SelectList GetCompaniesSelectList()
        {
            return new SelectList(_context.TransportCompanies, "CompanyId", "Name");
        }
    }
}
