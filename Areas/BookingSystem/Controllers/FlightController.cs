using GBC_Travel_Group_136.Data;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
	public class FlightController : Controller
    {
        private readonly AppDbContext _db;

        public FlightController(AppDbContext context)
        {
            _db = context;
        }

		[HttpGet("")]
		public async Task<IActionResult> Index()
		{
			var flights = await _db.Flights.ToListAsync();
			return View(flights);
		}


        [HttpGet("BookFlight/{flightId:int}/{seatId:int}")]
        public async Task<IActionResult> Book(int flightId, int seatId)
        {
            var flight = await _db.Flights
                .Include(s => s.Seats.Where(s => s.SeatId == seatId))
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("BookFlight", "UserBooking", flight);
            }

            return View("FlightBookingOptions", flight);
        }



        [HttpGet("Details/{id:int}")]
		public async Task<IActionResult> Details(int id)
		{
			var flights = await _db.Flights
				.Include(s => s.Seats)
				.FirstOrDefaultAsync(f => f.FlightId == id);

			if (flights == null)
			{
				return NotFound();
			}

			return View(flights);
		}

		[HttpGet("Create")]
		public IActionResult Create()
		{

			return View();
		}

		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Flight flight)
		{
			if (ModelState.IsValid)
			{
				_db.Flights.Add(flight);
				await _db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(flight);
		}

		[HttpGet("Edit/{id:int}")]
		public async Task<IActionResult> Edit(int id)
		{
			var flight = await _db.Flights.FindAsync(id);
			if (flight == null)
			{
				return NotFound();
			}
			return View(flight);
		}
		[HttpPost("Edit/{id:int}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("FlightId, Airline, Origin, Destination, Departure, Arrival")] Flight flight)
		{
			if (id != flight.FlightId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_db.Update(flight);
					await _db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await FlightExists(flight.FlightId))
					{
						return NotFound(flight);
					}
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(flight);
		}

		private async Task<bool> FlightExists(int id)
		{
			return await _db.Flights.AnyAsync(p => p.FlightId == id);
		}

		[HttpGet("Delete/{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var flight = await _db.Flights.FirstOrDefaultAsync(p => p.FlightId == id);
			if (flight == null)
			{
				return NotFound();
			}
			return View(flight);
		}
		[HttpPost("DeleteConfirmed/{Flightid:int}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int FlightId)
		{
			var flight = _db.Flights.Find(FlightId);
			if (flight != null)
			{
				_db.Flights.Remove(flight);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return NotFound();
		}

		[HttpGet("Search/{searchType}/{searchString?}")]
		public async Task<IActionResult> Search(string searchString, string searchType)
		{
			var projectsQuery = from p in _db.Flights
								select p;

			bool searchPerformed = !string.IsNullOrEmpty(searchString);

			if (searchPerformed)
			{
				projectsQuery = projectsQuery.Where(f => f.Airline.Contains(searchString)
											   || f.Origin.Contains(searchString)
											   || f.Destination.Contains(searchString));
			}

			var projects = await projectsQuery.ToListAsync();
			ViewData["SearchPerformed"] = searchPerformed;
			ViewData["SearchString"] = searchString;
			return View("Index", projects); // Reuse the Index view to display results
		}

		[HttpGet("Search/{flightId:int}/{searchString?}")]
		public async Task<IActionResult> Search(int flightId, string searchString)
		{
			var seatsQuery = _db.Seats.AsQueryable();
			if (!string.IsNullOrEmpty(searchString))
			{
				seatsQuery = seatsQuery
					.Where(t => t.SeatType.Contains(searchString));
			}
			var tasks = await seatsQuery.ToListAsync();
			ViewBag.ProjectId = flightId;
			return View("Index", tasks);
		}
	}
}
