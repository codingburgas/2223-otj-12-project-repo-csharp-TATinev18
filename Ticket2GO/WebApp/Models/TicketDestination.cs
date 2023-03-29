using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp.Models
{
    public class TicketDestination
    {
        public Ticket Tickets { get; set; }
        public Guid TicketId { get; set; }

        public Destination Destinations { get; set; }
        public Guid DestinationId { get; set; }
    }
}