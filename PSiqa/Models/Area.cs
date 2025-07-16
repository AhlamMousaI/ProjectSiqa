using System.ComponentModel.DataAnnotations;

namespace PSiqa.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المنطقة مطلوب")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        


        // الزبائن في هذه المنطقة
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();

        // الخزانات التي تغطي هذه المنطقة عبر جدول وسيط
        public ICollection<TankArea> TankAreas { get; set; } = new List<TankArea>();
    }
}
