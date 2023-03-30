using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Areas.Identity.Data;

namespace WebApp.Models
{
    public class TransportCompany
    {
        [Key]
        public Guid TransportCompanyId { get; set; } = Guid.NewGuid();

        public ICollection<TransportCompanyAspNetUser> TransportCompanyAspNetUsers { get; set; }

        [Required]
        public string Name { get; set; }

        [Column(TypeName = "varbinary(max)")]
        [Required]
        public byte[] Logo { get; set; }

        public ICollection<Bus> Buses { get; set; }

    }
}