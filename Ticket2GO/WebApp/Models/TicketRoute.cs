using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp.Models
{
    public class TicketRoute
    {
        [Key]
        public Guid TicketId { get; set; }
        [Key]
        public Guid RouteId { get; set; }
    }
}
