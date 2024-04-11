using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Hotel")]
        public string? HotelName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        
        public List<Room>? Rooms { get; set; }
    }
}
