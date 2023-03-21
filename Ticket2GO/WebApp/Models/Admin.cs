using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Areas.Identity.Data;
namespace WebApp.Models
{
    public class Admin
    {
        [Key]
        public Guid AdminId { get; set; } = Guid.NewGuid();
        public Guid Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
