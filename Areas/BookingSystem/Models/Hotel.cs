using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Hotel Name")]
        public string HotelName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Rooms")]
        public int Rooms { get; set; }
    }
}
