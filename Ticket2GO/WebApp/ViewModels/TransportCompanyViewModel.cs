using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class TransportCompanyViewModel
    {
        public Guid TransportCompanyId { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Моля, прикачете снимка.")]
        [Display(Name = "Logo")]
        public IFormFile Logo { get; set; }
    }
}
