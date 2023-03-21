using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Route
    {
        [Key]
        public Guid RouteId { get; set; } = Guid.NewGuid();

        public Guid BusId { get; set; }
        public Bus Bus { get; set; }

        public Guid TransportCompanyId { get; set; }
        public TransportCompany TransportCompany { get; set; }

        [Required]
        public string StartingDestination { get; set; }

        [Required]
        public string FinalDestination { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        [Required]
        public DateTime TimeOfArrival { get; set; }
    }
}
