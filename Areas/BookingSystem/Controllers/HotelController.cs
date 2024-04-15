using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GBC_Travel_Group_136.Areas.Identity;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
	public class HotelController : Controller
    {
        private readonly AppDbContext _db;

        public HotelController(AppDbContext context)
        {
            _db = context;
        }
        public async Task<IActionResult> Index(string searchString, string searchType)
        {
            var hotel = await _db.Hotels.ToListAsync();

			return View(hotel);
		}


        [HttpGet("Book/{hotelId:int}/{roomId:int}")]
        public async Task<IActionResult> Book(int hotelId, int roomId)
        {
            TempData["HotelId"] = hotelId;
            TempData["RoomId"] = roomId;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("BookHotel", "UserBooking");
            }

            return View("HotelBookingOptions");
        }


        [HttpGet("Details/{id:int}")]
		public async Task<IActionResult> Details(int id)
		{
			var hotel = await _db.Hotels
				.Include(h => h.Rooms)
				.FirstOrDefaultAsync(f => f.HotelId == id);
			
			if (hotel == null)
			{
				return NotFound();
			}
			
            return View(hotel);
		}


		[HttpGet("Create")]
		public IActionResult Create()
		{

			return View();
		}

		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Hotel hotel)
		{
			if (ModelState.IsValid)
			{
				_db.Hotels.Add(hotel);
				await _db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(hotel);
		}


		[HttpGet("Edit/{id:int}")]
		public async Task<IActionResult> Edit(int id)
		{
			var hotel = await _db.Hotels.FindAsync(id);
			if (hotel == null)
			{
				return NotFound();
			}
			return View(hotel);
		}
		[HttpPost("Edit/{id:int}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("HotelId, HotelName, Location, Description")] Hotel hotel)
		{
			if (id != hotel.HotelId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_db.Update(hotel);
					await _db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await HotelExists(hotel.HotelId))
					{
						return NotFound(hotel);
					}
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(hotel);
		}

		private async Task<bool> HotelExists(int id)
		{
			return await _db.Hotels.AnyAsync(p => p.HotelId == id);
		}



		[HttpGet("Delete/{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var hotel = await _db.Hotels.FirstOrDefaultAsync(p => p.HotelId == id);
			if (hotel == null)
			{
				return NotFound();
			}
			return View(hotel);
		}
		[HttpPost("DeleteConfirmed/{hotelid:int}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int hotelId)
		{
			var hotel = _db.Flights.Find(hotelId);
			if (hotel != null)
			{
				_db.Flights.Remove(hotel);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return NotFound();
		}



		[HttpGet("Search/{searchString?}")]
		public async Task<IActionResult> Search(string searchString)
		{
			var hotelQuery = from p in _db.Hotels
								select p;

			bool searchPerformed = !string.IsNullOrEmpty(searchString);

			if (searchPerformed)
			{
				hotelQuery = hotelQuery.Where(f => f.HotelName.Contains(searchString)
											   || f.Location.Contains(searchString)
											   || f.Description.Contains(searchString));
			}

			var projects = await hotelQuery.ToListAsync();
			ViewData["SearchPerformed"] = searchPerformed;
			ViewData["SearchString"] = searchString;
			return View("Index", projects); // Reuse the Index view to display results
		}

		[HttpGet("Search/{hotelId:int}/{searchString?}")]
		public async Task<IActionResult> Search(int hotelId, string searchString)
		{
			var roomsQuery = _db.Rooms.AsQueryable();
			if (!string.IsNullOrEmpty(searchString))
			{
				roomsQuery = roomsQuery
					.Where(t => t.RoomType.Contains(searchString));
			}
			var tasks = await roomsQuery.ToListAsync();
			ViewBag.ProjectId = hotelId;
			return View("Index", tasks);
		}
	}
}
