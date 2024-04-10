using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Airline")]
        [StringLength(50)]
        public string Airline { get; set; }

        [Required]
        [Display(Name = "Flight Number")]
        [StringLength(50)]
        public string FlightNumber { get; set; }

        [Required]
        [Display(Name = "Origin")]
        [StringLength(50)]
        public string Origin { get; set; }

        [Required]
        [Display(Name = "Destination")]
        [StringLength(50)]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Departure")]
        [DataType(DataType.DateTime)]
        public DateTime Departure { get; set; }

        [Required]
        [Display(Name = "Arrival")]
        [DataType(DataType.DateTime)]
        public DateTime Arrival { get; set; }

        [Required]
        [Display(Name = "Seats")]
        public int Seats { get; set; }

    }
}
