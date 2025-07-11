using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PSiqa.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int TankId { get; set; }

        [Required]
        public int Quantity { get; set; } // عدد البراميل المطلوبة

        [Required]
        [StringLength(50)]
        public string Status { get; set; } //"معلق"، "جاري التسليم"، "تم"
        [Required]
        public DateTime OrderTime { get; set; }
        public int? DriverId { get; set; }

        // علاقات
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [ForeignKey("TankId")]
        public Tank Tank { get; set; }

        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
    }
}
