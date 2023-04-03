using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class CreateBusViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int SeatsNumber { get; set; }

        public Guid TransportCompanyId { get; set; }
    }
}

