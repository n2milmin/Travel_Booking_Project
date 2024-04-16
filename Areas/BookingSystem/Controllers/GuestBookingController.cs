using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
    [Area("BookingSystem")]
    [Route("[area]/[controller]/[action]")]
    public class GuestBookingController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<GuestBookingController> _logger;

        public GuestBookingController(
            AppDbContext context, 
            ILogger<GuestBookingController> logger)
        {
            _db = context;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();

        }


        [HttpGet("BookCar")]
        public async Task<IActionResult> BookCar()
        {
            int carId = (int)TempData["CarId"];
            //var userId = TempData["userId"];

            TempData["CarId"] = null; // Clear TempData after retrieving values
            //TempData["userId"] = null;

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


            if (TempData["userId"] is string userIdString)
            {
                var userId = userIdString;
                Booking result = new Booking();
                result.UserId = userId;
                result.ServiceId = 1;
                result.Car = car;

                _logger.LogInformation($"WORKED {userId}");

                return View(result);
            }
            else
            {
                Booking result = new Booking();
                result.UserId = "";
                result.ServiceId = 1;
                result.Car = car;

                _logger.LogInformation($"failed0");

                return View(result);
            }



        }

        [HttpPost("BookCar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookCar(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var carChange = await _db.Cars.FindAsync(booking.Car);
                carChange.Available = false;
                _db.Entry(carChange).State = EntityState.Modified;

                _logger.LogInformation($"Finished booking car with id: {booking.Car.CarId}");

                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction("SuccessfulBooking");
            }
            return View(booking);
        }


        [HttpGet("BookFlight")]
        public async Task<IActionResult> BookFight()
        {
            int flightId = (int)TempData["FlightId"];
            int seatId = (int)TempData["SeatId"];
            //var userId = (string)TempData["userId"];

            TempData["FlightId"] = null; // Clear TempData after retrieving values
            TempData["SeatId"] = null;
            //TempData["userId"] = null;

            _logger.LogInformation($"Starting booking flight {flightId}, seat {seatId}");

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

            if (TempData["userId"] is string userIdString)
            {
                var userId = userIdString;
                Booking result = new Booking();
                result.UserId = userId;
                result.ServiceId = 2;
                result.Flight = flight;
                result.Seats = flight.Seats[0];

                _logger.LogInformation($"WORKED {userId}");

                return View(result);
            }
            else
            {
                Booking result = new Booking();
                result.UserId = "";
                result.ServiceId = 2;
                result.Flight = flight;
                result.Seats = flight.Seats[0];

                _logger.LogInformation($"failed0");

                return View(result);
            }
        }

        [HttpPost("BookFlight")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookFlight(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var seatChange = await _db.Seats.FindAsync(booking.Seats.SeatId);
                seatChange.Amount = booking.Seats.Amount;
                _db.Entry(seatChange).State = EntityState.Modified;

                _logger.LogInformation($"Finished booking flight {booking.Flight.FlightId}, seat {booking.Seats.SeatId}");

                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction("SuccessfulBooking");
            }
            return View(booking);
        }


        [HttpGet("BookHotel")]
        public async Task<IActionResult> BookHotel()
        {
            int hotelId = (int)TempData["HotelId"];
            int roomId = (int)TempData["RoomId"];
            //var userId = (string)TempData["userId"];

            TempData["HotelId"] = null; // Clear TempData after retrieving values
            TempData["RoomId"] = null;
            //TempData["userId"] = null;

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

            if (TempData["userId"] is string userIdString)
            {
                var userId = userIdString;
                Booking result = new Booking();
                result.UserId = userId;
                result.ServiceId = 3;
                result.Hotel = hotel;
                result.Rooms = hotel.Rooms[0];

                _logger.LogInformation($"WORKED {userId}");

                return View(result);
            }
            else
            {
                Booking result = new Booking();
                result.UserId = "";
                result.ServiceId = 3;
                result.Hotel = hotel;
                result.Rooms = hotel.Rooms[0];

                _logger.LogInformation($"failed0");

                return View(result);
            }
        }



        [HttpPost("BookHotel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookHotel(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var roomChange = await _db.Rooms.FindAsync(booking.Rooms.RoomId);
                roomChange.Amount = booking.Rooms.Amount;
                _db.Entry(roomChange).State = EntityState.Modified;

                _logger.LogInformation($"Finished booking hotel with id: {booking.Hotel.HotelId}, room {booking.Seats.SeatId}");

                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction("BookingConfirmation", booking);
            }
            return View("BookHotel", booking);
        }

    }
}
