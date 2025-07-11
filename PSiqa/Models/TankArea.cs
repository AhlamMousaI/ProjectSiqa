using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSiqa.Models
{
    public class TankArea
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int TankId { get; set; }

        [Required]
        public int AreaId { get; set; }

        [ForeignKey("TankId")]
        public Tank Tank { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; set; }

    }
}
