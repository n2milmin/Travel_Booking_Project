using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
	public class Booking
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "Booking Id")]
		public int BookingId { get; set; }

		[Display(Name = "User Id")]
		[DataType("nvarchar(450)")]
		public string UserId { get; set; }

		[EmailAddress]
		[Display(Name = "Email Address")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Service Id")]
		public int ServiceId { get; set; }

		[Required]
		[Display(Name = "Booking Date")]
		public DateTime BookingDate { get; set; }

		[Required]
		[Display(Name = "Check in Date")]
		public DateTime CheckInDate { get; set; }

		[Required]
		[Display(Name = "Check out Date")]
		public DateTime CheckOutDate { get; set; }



		[Display(Name = "Number of Rooms")]
		public int NumberOfRooms { get; set; }

		[Display(Name = "Number of Seats")]
		public int NumberOfSeats { get; set; }


		[Display(Name = "Flight")]
		public Flight? Flight { get; set; }
		
		[Display(Name = "Seats")]
		public Seat? Seats { get; set; }

		[Display(Name = "Hotel")]
		public Hotel? Hotel { get; set; }

		[Display(Name = "Rooms")]
		public Room? Rooms { get; set; }

		[Display(Name = "Car")]
		public Car? Car { get; set; }
	}
}
