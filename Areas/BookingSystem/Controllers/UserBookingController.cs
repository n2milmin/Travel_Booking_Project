﻿using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Authorization;
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

		public UserBookingController(AppDbContext context)
		{
			_db = context;
		}
		public IActionResult Index()
		{
			return View();
		}



		[HttpGet("BookCar")]
		public async Task<IActionResult> BookCar()
		{
            int carId = (int)TempData["CarId"];

            TempData["CarId"] = null; // Clear TempData after retrieving values

            var car = await _db.Cars.FindAsync(carId);

            if (!ModelState.IsValid)
			{
				return View();
			}

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
                var carChange = await _db.Cars.FindAsync(booking.Car);
                carChange.Available = false;
                _db.Entry(carChange).State = EntityState.Modified;

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

            TempData["FlightId"] = null; // Clear TempData after retrieving values
            TempData["SeatId"] = null;

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
                var seatChange = await _db.Seats.FindAsync(booking.Seats.SeatId);
                seatChange.Amount = booking.Seats.Amount;
                _db.Entry(seatChange).State = EntityState.Modified;

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

            TempData["HotelId"] = null; // Clear TempData after retrieving values
            TempData["RoomId"] = null;

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
                var roomChange = await _db.Rooms.FindAsync(booking.Rooms.RoomId);
                roomChange.Amount = booking.Rooms.Amount;
                _db.Entry(roomChange).State = EntityState.Modified;

                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return RedirectToAction("BookingConfirmation", booking);
            }
            return View("BookHotel", booking);
        }

    }
}
