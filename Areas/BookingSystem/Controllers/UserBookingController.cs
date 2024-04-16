using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using GBC_Travel_Group_136.Data;
using GBC_Travel_Group_136.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
	[Authorize]
	public class UserBookingController : Controller
	{
		private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserBookingController> _logger;

		public UserBookingController(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<UserBookingController> logger)
		{
			_db = context;
            _userManager = userManager;
            _logger = logger;
		}
        [HttpPost("Index")]
		public async Task<IActionResult> Index()
		{
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _db.Bookings.ToListAsync();

            bookings = bookings.Where(x => x.UserId == user.Id).ToList();

            return View(bookings);
		}



		[HttpGet("BookCar")]
		public async Task<IActionResult> BookCar()
		{
            int carId = (int)TempData["CarId"];

            TempData["CarId"] = null; // Clear TempData after retrieving values

            _logger.LogInformation($"Starting booking car with id: {carId}");
            
            var car = await _db.Cars.FindAsync(carId);

            if (!ModelState.IsValid)
			{
				return View();
			}

			if (car == null)
			{
				return NotFound();
			}

            var user = await _userManager.GetUserAsync(User);

			Booking result = new Booking();
            result.UserId = user.Id;
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
                try
                {
                    var carChange = await _db.Cars.FindAsync(booking.Car);
                    if (carChange != null)
                    {
                        carChange.Available = false;
                        _db.Entry(carChange).State = EntityState.Modified;

                        _db.Bookings.Add(booking);
                        await _db.SaveChangesAsync();

                        _logger.LogInformation($"Finished booking car with id: {carChange.CarId}");

                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return PartialView("_BookingConfirmationPartial", booking);
                        }

                        return View("BookingConfirmation", booking);
                    }
                    else
                    {
                        _logger.LogError($"Car with ID {booking.Car} not found.");
                        ModelState.AddModelError("", "Car not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error booking car: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while processing your booking. Please try again.");
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            }

            return View("BookCar", booking);
        }


        [HttpGet("BookFlight")]
        public async Task<IActionResult> BookFight()
        {
            int flightId = (int)TempData["FlightId"];
            int seatId = (int)TempData["SeatId"];

            TempData["FlightId"] = null; // Clear TempData after retrieving values
            TempData["SeatId"] = null;

            _logger.LogInformation($"Starting booking flight {flightId} seat {seatId}");

            var flight = await _db.Flights
                .Include(s => s.Seats.Where(s => s.SeatId == seatId))
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (flight == null)
            {
                return NotFound();
            }

            Booking result = new Booking();
            result.ServiceId = 2;
            result.Flight = flight;
            result.Seats = flight.Seats[0];

            return View(result);
        }

        [HttpPost("BookFlight")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookFlight(Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var seatChange = await _db.Seats.FindAsync(booking.Seats.SeatId);
                    if (seatChange != null)
                    {
                        seatChange.Amount -= booking.Seats.Amount;
                        _db.Entry(seatChange).State = EntityState.Modified;

                        _logger.LogInformation($"Finished booking flight {booking.Flight.FlightId}, seat {booking.Seats.SeatId}");

                        _db.Bookings.Add(booking);
                        await _db.SaveChangesAsync();

                        _logger.LogInformation($"Finished booking flight {booking.Flight.FlightId}, seat {booking.Seats.SeatId}");

                        return View("BookingConfirmation", booking);
                    }
                    else
                    {
                        _logger.LogError($"Seat with ID {booking.Seats} not found.");
                        ModelState.AddModelError("", "Seat not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error booking car: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while processing your booking. Please try again.");
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            }

            return View("BookCar", booking);
        }


        [HttpGet("BookHotel")]
        public async Task<IActionResult> BookHotel()
        {
            int hotelId = (int)TempData["HotelId"];
            int roomId = (int)TempData["RoomId"];

            TempData["HotelId"] = null; // Clear TempData after retrieving values
            TempData["RoomId"] = null;

            _logger.LogInformation($"Starting booking hotel with id: {hotelId}, room {roomId}");

            var hotel = await _db.Hotels
                .Include(r => r.Rooms.Where(r => r.RoomId == roomId))
                .FirstOrDefaultAsync(h => h.HotelId == hotelId);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (hotel == null)
            {
                return NotFound();
            }

            Booking result = new Booking();
            result.ServiceId = 3;
            result.Hotel = hotel;
            result.Rooms = hotel.Rooms[0];

            return View(result);
        }

        

        [HttpPost("BookHotel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookHotel(Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var roomChange = await _db.Rooms.FindAsync(booking.Rooms.RoomId);
                    if (roomChange != null)
                    {
                        roomChange.Amount -= booking.Rooms.Amount;
                        _db.Entry(roomChange).State = EntityState.Modified;

                        _logger.LogInformation($"Finished booking hotel with id: {booking.Hotel.HotelId}, room {booking.Rooms.RoomId}");

                        _db.Bookings.Add(booking);
                        await _db.SaveChangesAsync();

                        _logger.LogInformation($"Finished booking hotel {booking.Hotel.HotelId}, seat {booking.Rooms.RoomId}");

                        return View("BookingConfirmation", booking);
                    }
                    else
                    {
                        _logger.LogError($"Seat with ID {booking.Seats} not found.");
                        ModelState.AddModelError("", "Seat not found. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error booking car: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while processing your booking. Please try again.");
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            }

            return View("BookCar", booking);
        }

    }
}
