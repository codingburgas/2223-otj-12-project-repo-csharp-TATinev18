using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels
{
    public class CreateDestinationViewModel
    {
        [Required]
        [Display(Name = "Начална дестинация")]
        public string StartingDestination { get; set; }

        [Required]
        [Display(Name = "Крайна дестинация")]
        public string FinalDestination { get; set; }

        [Required]
        [TimeSpan(ErrorMessage = "Продължителността трябва да бъде между 00:00:01 и 24:00:00.")]
        [Display(Name = "Продължителност")]
        public TimeSpan Duration { get; set; }

        [Required]
        [Display(Name = "Дата на заминаване")]
        public DateTime Departure { get; set; }

        [Required]
        [Display(Name = "Дата на пристигане")]
        public DateTime TimeOfArrival { get; set; }

        [Display(Name = "Автобусна компания")]
        public Guid? SelectedCompanyId { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; } = Enumerable.Empty<SelectListItem>();

        [Display(Name = "Автобус")]
        public Guid? SelectedBusId { get; set; }

        [Display(Name = "Ден от седмицата")]
        public DayOfWeek? RepeatingDayOfWeek { get; set; }

        [Display(Name = "Брой на повторения")]
        public int? NumberOfRepetitions { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, 9999.99, ErrorMessage = "Цената трябва да бъде между 0.01 and 9999.99")]
        public decimal TotalPrice { get; set; }

        public bool DeleteAllRepetitions { get; set; }

        public IEnumerable<SelectListItem> Buses { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}