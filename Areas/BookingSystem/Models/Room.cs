using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Room Type")]
        public string? RoomType { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        public int HotelId { get; set; }
    }
}
