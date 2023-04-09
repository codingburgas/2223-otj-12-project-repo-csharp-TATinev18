using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class CreateDestinationViewModel
    {
        [Required]
        [Display(Name = "Starting Destination")]
        public string StartingDestination { get; set; }

        [Required]
        [Display(Name = "Final Destination")]
        public string FinalDestination { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public TimeSpan Duration { get; set; }

        [Required]
        [Display(Name = "Departure")]
        public DateTime Departure { get; set; }

        [Required]
        [Display(Name = "Time Of Arrival")]
        public DateTime TimeOfArrival { get; set; }

        [Display(Name = "Transport Company")]
        public Guid? SelectedCompanyId { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name = "Bus")]
        public Guid? SelectedBusId { get; set; }

        public IEnumerable<SelectListItem> Buses { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}