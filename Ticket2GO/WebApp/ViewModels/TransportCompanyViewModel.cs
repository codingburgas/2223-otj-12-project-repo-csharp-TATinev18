using System.ComponentModel.DataAnnotations;

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
