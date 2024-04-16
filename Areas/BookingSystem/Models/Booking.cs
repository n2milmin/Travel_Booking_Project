using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
	public class Booking
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
		[Required]
		[DataType("nvarchar(450)")]
		public string UserId { get; set; }
		[Required]
		public int ServiceId { get; set; }
		[Required]
		public DateTime BookingDate { get; set; }
		[Required]
		public DateTime CheckInDate { get; set; }
		[Required]
		public DateTime CheckOutDate { get; set; }

		
		public int NumberOfGuests { get; set; }
		public int NumberOfRooms { get; set; }
		public int NumberOfSeats { get; set; }


		public Flight? Flight { get; set; }
		public Seat? Seats { get; set; }
		public Hotel? Hotel { get; set; }
		public Room? Rooms { get; set; }
		public Car? Car { get; set; }
	}
}
