using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
	[Authorize]
	public class UserBookingController : Controller
	{
		private readonly AppDbContext _db;

		public UserBookingController(AppDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			return View();
		}



		[HttpGet("BookCar/{carId:int}")]
		public async Task<IActionResult> BookCar(int carId)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var car = await _db.Cars.FindAsync(carId);
			if (car == null)
			{
				return NotFound();
			}

			Booking result = new Booking();
			result.ServiceId = 1;
			result.Car = car;

			return View(result);
		}

		[HttpPost("BookCar")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> BookCar(Booking booking)
		{
			if (ModelState.IsValid)
			{
				_db.Bookings.Add(booking);
				await _db.SaveChangesAsync();
				return RedirectToAction("SuccessfulBooking");
			}
			return View(booking);
		}


        [HttpGet("BookFlight/{flightId:int}/{seatId:int}")]
        public async Task<IActionResult> BookFight(int flightId, int seatId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var flight = await _db.Flights
                .Include(s => s.Seats.Where(s => s.SeatId == seatId))
                .FirstOrDefaultAsync(f => f.FlightId == flightId);
            if (flight == null)
            {
                return NotFound();
            }

            Booking result = new Booking();
            result.ServiceId = 2;
            result.Flight = flight;

            return View(result);
        }

        [HttpPost("BookFlight")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookFlight(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction("SuccessfulBooking");
            }
            return View(booking);
        }


        [HttpGet("BookHotel/{hotelId:int}/{roomId:int}")]
        public async Task<IActionResult> BookHotel(int hotelId, int roomId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var hotel = await _db.Hotels
                .Include(r => r.Rooms.Where(r => r.RoomId == roomId))
                .FirstOrDefaultAsync(h => h.HotelId == hotelId);
            if (hotel == null)
            {
                return NotFound();
            }

            Booking result = new Booking();
            result.ServiceId = 3;
            result.Hotel = hotel;

            return View(result);
        }

        [HttpPost("BookHotel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookHotel(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction("SuccessfulBooking");
            }
            return View(booking);
        }
    }
}
