using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company")]
        [StringLength(100)]
        public string? Company { get; set; }

        [Required]
        [Display(Name = "Location")]
        [StringLength(100)]
        public string? Location { get; set; }

        [Required]
        [Display(Name = "Make")]
        [StringLength(100)]
        public string? Make { get; set; }

        [Required]
        [Display(Name = "Model")]
        [StringLength(100)]
        public string? Model { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool Available { get; set; } = true;
    }
}
