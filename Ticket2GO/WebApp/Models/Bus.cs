using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Bus
    {
        [Key]
        public Guid BusId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public Guid TransportCompanyId { get; set; }

        [ForeignKey("TransportCompanyId")]
        public TransportCompany TransportCompany { get; set; }

        [Column(TypeName = "tinyint")]
        public int SeatsNumber { get; set; }

    }
}
