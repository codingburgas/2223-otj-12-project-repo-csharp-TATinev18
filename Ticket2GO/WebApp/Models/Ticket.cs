using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models
{
    public class Ticket
    {
        [Key]
        public Guid TicketId { get; set; } = Guid.NewGuid();

        [Required]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [Required]
        public decimal Price { get; set; }

        public ICollection<TicketDestination> TicketDestinations { get; set; } = new HashSet<TicketDestination>();
    }
}
