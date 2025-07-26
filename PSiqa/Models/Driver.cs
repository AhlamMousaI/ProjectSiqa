using System.ComponentModel.DataAnnotations;

namespace PSiqa.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Phone]
        public string Phone { get; set; }

        // الطلبات التي ينفذها هذا السائق
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
