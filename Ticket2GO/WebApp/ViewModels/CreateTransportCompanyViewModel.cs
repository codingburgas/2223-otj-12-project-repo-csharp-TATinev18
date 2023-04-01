using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CreateTransportCompanyViewModel
    {
        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Logo")]
        public IFormFile Logo { get; set; }
    }
}
