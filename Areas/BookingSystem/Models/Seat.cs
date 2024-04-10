using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Seat Type")]
        public string? SeatType { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public int Amount { get; set;}

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set;}
    }
}
