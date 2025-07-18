using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSiqa.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "رقم الهاتف غير صالح. يجب أن يبدأ بـ 05 ويتكون من 10 أرقام")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "العنوان يجب أن يكون بين 5 و 100 حرف")]
        public string Address { get; set; }

        [Required(ErrorMessage = "يجب اختيار المنطقة")]
        public int AreaId { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
