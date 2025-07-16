using System.ComponentModel.DataAnnotations;

namespace PSiqa.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // العلاقة مع المنطقة
        [Required]
        public int AreaId { get; set; }
        public Area? Area { get; set; }

        // الطلبات المرتبطة بهذا الزبون
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
