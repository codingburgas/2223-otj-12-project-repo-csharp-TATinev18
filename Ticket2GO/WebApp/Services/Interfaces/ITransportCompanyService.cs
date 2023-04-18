using WebApp.Areas.Identity.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Services.Interfaces
{
    public interface ITransportCompanyService
    {
        public Task<List<TransportCompany>> GetTransportCompanies(ApplicationUser user);
        public Task CreateTransportCompany(TransportCompanyViewModel viewModel, ApplicationUser user);
        public TransportCompanyAspNetUser? GetCurrentUsersTransportCompany(ApplicationUser currentUser);
        public Task CreateBus(Guid transportCompanyId, CreateBusViewModel viewModel);
        public Task<TransportCompany?> GetTranspoerCompanyDeatils(Guid id);
        public bool TransportCompanyExists(Guid id);
        public Task<TransportCompany?> GetTransportCompany(Guid transportCompanyId);
        public IQueryable<Bus> GetCompanyBuses(Guid transportCompanyId);
        public Task DeleteTransportCompany(Guid id, TransportCompany? transportCompany);
        public Task<List<Destination>> GetDestinations(Guid id);
        public TransportCompanyViewModel GenerateTransportCompnayViewModel(TransportCompany? transportCompany);
        public Task EditTransportCompany(TransportCompanyViewModel viewModel, TransportCompany? transportCompany);
        public Task<List<TransportCompany>> GetAllTransportCompanies();
    }
}
