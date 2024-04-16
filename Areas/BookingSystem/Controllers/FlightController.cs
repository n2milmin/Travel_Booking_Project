using GBC_Travel_Group_136.Data;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GBC_Travel_Group_136.Filters;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
	public class FlightController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FlightController> _logger;

        public FlightController(AppDbContext context, ILogger<FlightController> logger)
        {
            _db = context;
			_logger = logger;
        }

		[HttpGet("")]
		public async Task<IActionResult> Index()
		{
            _logger.LogInformation($"Viewing Flight list");

            var flights = await _db.Flights.ToListAsync();
			return View(flights);
		}


        [HttpGet("Book/{flightId:int}/{seatId:int}")]
        public async Task<IActionResult> Book(int flightId, int seatId)
        {
            _logger.LogInformation($"Starting booking a Flight {flightId} Seat {seatId}");

            TempData["FlightId"] = flightId;
            TempData["SeatId"] = seatId;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("BookFlight", "UserBooking");
            }

            return View("FlightBookingOptions");
        }



        [HttpGet("Details/{id:int}")]
		public async Task<IActionResult> Details(int id)
		{
            _logger.LogInformation($"Viewing details of Flight : {id}");

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
            _logger.LogInformation($"Start Creating a Flight");

            return View();
		}

		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Flight flight)
		{
            _logger.LogInformation($"Finished creating flight : {flight.FlightId}");

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
            _logger.LogInformation($"Starting edditing a flight with id: {id}");

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
            _logger.LogInformation($"Finished editing a car with id: {id}");

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
            _logger.LogInformation($"Starting deleting a flight with id: {id}");

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
            _logger.LogInformation($"Finished deleting flight with id: {FlightId}");

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
		public async Task<IActionResult> Search(string searchString)
		{
            _logger.LogInformation($"Searching flights : {searchString}");

            var projectsQuery = from p in _db.Flights
								select p;

			bool searchPerformed = !string.IsNullOrEmpty(searchString);

			if (searchPerformed)
			{
				projectsQuery = projectsQuery.Where(f => f.Airline.Contains(searchString)
											   || f.Origin.Contains(searchString)
											   || f.Destination.Contains(searchString));
			}

			var flights = await projectsQuery.ToListAsync();
			ViewData["SearchPerformed"] = searchPerformed;
			ViewData["SearchString"] = searchString;
            _logger.LogInformation($"Result : {flights}");
            return View("Index", flights); // Reuse the Index view to display results
		}

		[HttpGet("Search/{flightId:int}/{searchString?}")]
		public async Task<IActionResult> Search(int flightId, string searchString)
		{
            _logger.LogInformation($"Searching flights : {searchString}");

            var seatsQuery = _db.Seats.AsQueryable();
			if (!string.IsNullOrEmpty(searchString))
			{
				seatsQuery = seatsQuery
					.Where(t => t.SeatType.Contains(searchString));
			}
			var tasks = await seatsQuery.ToListAsync();
			ViewBag.FlightId = flightId;
            _logger.LogInformation($"Searching result : {flightId}");

            return View("Index", tasks);
		}
	}
}
