using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp.Models
{
    public class TicketDestination
    {
        public Ticket Ticket { get; set; }
        
        [Required]
        public Guid TicketId { get; set; }

        public Destination Destination { get; set; }
        [Required]
        public Guid DestinationId { get; set; }
    }
}