using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class SelectSeatViewModel
    {
        public Guid DestinationId { get; set; }
        public string StartingDestination { get; set; }
        public string FinalDestination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime TimeOfArrival { get; set; }
        public decimal Price { get; set; }
        public string BusName { get; set; }
        public string TransportCompany { get; set; }
        public int MaxSeats { get; set; }
        public string ReturnDestinationId { get; set; }
        public Guid? SelectedReturnDestinationId { get; set; }
        public IEnumerable<SelectListItem> ReturnDestinations { get; set; }
        public IEnumerable<int> AvailableSeats { get; set; }
        [Required(ErrorMessage = "Моля, изберете място.")]
        public int? SelectedSeat { get; set; }
    }
}
