using WebApp.Areas.Identity.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Services.Interfaces
{
    public interface IDestinationService
    {
        public Task<List<Destination>> GetDestinations();
        public CreateDestinationViewModel GetCompanies(ApplicationUser user);
        public Task<CreateDestinationViewModel> CreateDestination(Guid companyId);
        public Task<Destination?> GetDestination(Guid? id);
        public Task DeleteDestination(bool deleteAllRepetitions, Destination? destination);
        public Task CreateDestination(CreateDestinationViewModel viewModel);
        public void GetBuses(CreateDestinationViewModel viewModel, string companyId);
    }
}
