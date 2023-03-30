using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Destination
    {
        [Key]
        public Guid DestinationId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BusId { get; set; }
        [ForeignKey("BusId")]
        public Bus Bus { get; set; }

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

        public ICollection<TicketDestination> TicketDestinations { get; set; } = new HashSet<TicketDestination>();
    }
}
