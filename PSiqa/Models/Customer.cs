using System.ComponentModel.DataAnnotations;

namespace PSiqa.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string Name { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(200, ErrorMessage = "يجب ألا يتجاوز العنوان 200 حرف")]
        public string Address { get; set; }

        [Required(ErrorMessage = "يجب اختيار منطقة")]
        public int AreaId { get; set; }

        public Area Area { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

