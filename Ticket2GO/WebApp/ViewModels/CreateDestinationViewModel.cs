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

        [Display(Name = "Repeating Day Of Week")]
        public DayOfWeek? RepeatingDayOfWeek { get; set; }

        [Display(Name = "Number of Repetitions")]
        public int? NumberOfRepetitions { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal TotalPrice { get; set; }

        public bool DeleteAllRepetitions { get; set; }

        public IEnumerable<SelectListItem> Buses { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}