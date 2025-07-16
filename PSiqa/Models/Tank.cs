using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSiqa.Models
{
    public class Tank
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TankName { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; } // السعة بالبراميل مثلاً

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string WaterType { get; set; } = string.Empty; // مثل: شرب، زراعي

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerUnit { get; set; } // سعر البرميل أو الوحدة

        // الطلبات التي تم طلبها من هذا الخزان
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        // المناطق التي يغطيها هذا الخزان (عبر جدول وسيط)
        public ICollection<TankArea> TankAreas { get; set; } = new List<TankArea>();
    }
}
