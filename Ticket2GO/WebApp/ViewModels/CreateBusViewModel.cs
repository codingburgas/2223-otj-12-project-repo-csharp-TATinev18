using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class CreateBusViewModel
    {
        [Required(ErrorMessage ="Въведете име")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Въведете броя на местата")]
        public int SeatsNumber { get; set; }    

        public Guid TransportCompanyId { get; set; }
    }
}

