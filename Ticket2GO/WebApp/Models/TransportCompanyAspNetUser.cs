using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models
{
    public class TransportCompanyAspNetUser
    {
        [Required]
        public Guid TransportCompanyId { get; set; }
        public TransportCompany TransportCompany { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
