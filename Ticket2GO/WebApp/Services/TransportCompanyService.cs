using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class TransportCompanyService : ITransportCompanyService
    {
        private readonly ApplicationDbContext _context;
        public TransportCompanyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TransportCompany>> GetTransportCompanies(ApplicationUser user)
        {
            return await _context.TransportCompaniesAspNetUsers
                                                          .Include(tc => tc.TransportCompany)
                                                          .Where(tc => tc.ApplicationUserId == user.Id)
                                                          .Select(tc => tc.TransportCompany)
                                                          .ToListAsync();
        }

        public async Task<bool> CreateTransportCompany(TransportCompanyViewModel viewModel, ApplicationUser user)
        {
            if (string.IsNullOrEmpty(viewModel.Name) || viewModel.Logo == null)
            {
                return false;
            }

            if (_context.TransportCompanies.Any(tc => tc.Name == viewModel.Name))
            {
                return false;
            }

            byte[] logoBytes;

            using (var memoryStream = new MemoryStream())
            {
                await viewModel.Logo.CopyToAsync(memoryStream);
                logoBytes = memoryStream.ToArray();
            }

            var transportCompany = new TransportCompany
            {
                Name = viewModel.Name,
                Logo = logoBytes,
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now
            };

            _context.Add(transportCompany);
            await _context.SaveChangesAsync();

            if (user == null || transportCompany == null)
            {
                return false;
            }

            var transportCompanyAspNetUser = new TransportCompanyAspNetUser
            {
                TransportCompanyId = transportCompany.TransportCompanyId,
                ApplicationUserId = user.Id
            };

            _context.TransportCompaniesAspNetUsers.Add(transportCompanyAspNetUser);
            await _context.SaveChangesAsync();

            return true;
        }


        public TransportCompanyAspNetUser? GetCurrentUsersTransportCompany(ApplicationUser currentUser)
        {
            return _context.TransportCompaniesAspNetUsers
                            .FirstOrDefault(tc => tc.ApplicationUserId == currentUser.Id);
        }

        public async Task CreateBus(Guid transportCompanyId, CreateBusViewModel viewModel)
        {
            var bus = new Bus
            {
                Name = viewModel.Name,
                SeatsNumber = viewModel.SeatsNumber,
                TransportCompanyId = transportCompanyId
            };
            _context.Add(bus);
            await _context.SaveChangesAsync();
        }

        public async Task<TransportCompany?> GetTranspoerCompanyDeatils(Guid id)
        {
            return await _context.TransportCompanies
                            .FirstOrDefaultAsync(m => m.TransportCompanyId == id);
        }

        public bool TransportCompanyExists(Guid id)
        {
            return _context.TransportCompanies.Any(e => e.TransportCompanyId == id);
        }

        public async Task<TransportCompany?> GetTransportCompany(Guid transportCompanyId)
        {
            return await _context.TransportCompanies.FindAsync(transportCompanyId);
        }

        public IQueryable<Bus> GetCompanyBuses(Guid transportCompanyId)
        {
            return _context.Buses.Where(b => b.TransportCompanyId == transportCompanyId);
        }

        public async Task DeleteTransportCompany(Guid id, TransportCompany? transportCompany)
        {
            var transportCompaniesAspNetUsers = _context.TransportCompaniesAspNetUsers
                            .Where(tc => tc.TransportCompanyId == id);
            _context.TransportCompaniesAspNetUsers.RemoveRange(transportCompaniesAspNetUsers);

            if (transportCompany != null)
            {
                _context.TransportCompanies.Remove(transportCompany);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Destination>> GetDestinations(Guid id)
        {
            return await _context.Destinations
                            .Where(d => d.Bus.TransportCompanyId == id)
                            .ToListAsync();
        }

        public TransportCompanyViewModel GenerateTransportCompnayViewModel(TransportCompany? transportCompany)
        {

            if (transportCompany == null)
            {
                return new TransportCompanyViewModel(); ;
            }

            return new TransportCompanyViewModel
            {
                TransportCompanyId = transportCompany.TransportCompanyId,
                Name = transportCompany.Name
            };
        }

        public async Task<bool> EditTransportCompany(TransportCompanyViewModel viewModel, TransportCompany? transportCompany)
        {
            if (transportCompany == null)
            {
                return false;
            }

            var existingCompany = await _context.TransportCompanies
                .FirstOrDefaultAsync(c => c.Name == viewModel.Name && c.TransportCompanyId != viewModel.TransportCompanyId);

            if (existingCompany != null)
            {
                return false;
            }

            transportCompany.Name = viewModel.Name;
            transportCompany.DateEdited = DateTime.Now;

            if (viewModel.Logo != null)
            {
                using var memoryStream = new MemoryStream();
                await viewModel.Logo.CopyToAsync(memoryStream);
                transportCompany.Logo = memoryStream.ToArray();
            }

            _context.Update(transportCompany);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TransportCompany>> GetAllTransportCompanies()
        {
            return await _context.TransportCompanies.ToListAsync();
        }
    }
}
