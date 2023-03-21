using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Bus
    {
        [Key]
        public Guid BusId { get; set; } = Guid.NewGuid();
        public Guid TransportCompanyId { get; set; }
        public TransportCompany TransportCompany { get; set; }

        [Column(TypeName = "tinyint")]
        public int SeatsNumber { get; set; }


    }
}
