using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class BookTicketViewModel
    {
        public BookTicketViewModel()
        {
            Destinations = new List<Destination>();
            Companies = new SelectList(new List<TransportCompany>(), "Id", "Name");
            var defaultOption = new SelectListItem { Value = "", Text = "-- Select a company --", Selected = true };
            var companiesList = new List<SelectListItem> { defaultOption };
            Companies = new SelectList(companiesList.Concat(Companies), "Value", "Text");
        }

        public DateTime SelectedDate { get; set; }
        public Guid? SelectedCompanyId { get; set; }
        public Guid SelectedOriginDestinationId { get; set; }
        public string SelectedStartingDestination { get; set; }
        public string SelectedFinalDestination { get; set; }
        public IEnumerable<SelectListItem> AvailableDestinations { get; set; }

        public int SelectedSeat { get; set; }
        public int MaxSeats { get; set; }

        public bool IsRoundTrip { get; set; }
        public List<Destination> Destinations { get; set; }
        public SelectList Companies { get; set; }

        public DateTime? SelectedReturnDate { get; set; }
        public int? SelectedReturnSeat { get; set; }

        public IEnumerable<SelectListItem> ReturnDestinations { get; set; }
    }
}
