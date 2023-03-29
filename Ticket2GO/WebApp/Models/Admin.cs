using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Areas.Identity.Data;
namespace WebApp.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminId { get; set; } = Guid.NewGuid();

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
