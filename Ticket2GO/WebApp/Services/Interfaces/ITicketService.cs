using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Services.Interfaces
{
    public interface ITicketService
    {
        public Task<BookTicketViewModel> GetTickets(string transportCompanyId, string startingDestination, string finalDestination, DateTime? date);
        public Task BookTicket(BookTicketViewModel model, string userId);
        public void GetReturnDestinations(BookTicketViewModel model);
        public Task<SelectSeatViewModel> GenerateSelectSeatViewModel(Guid id, Destination? destination);
        public Task<Destination?> GetDestinations(Guid id);
        public Task ConfirmTicket(SelectSeatViewModel model, string userId);
        public Task<List<Ticket>?> GetTickets(string userId);
        public Task<bool> Cancel(Guid ticketId, string userId);
    }
}
