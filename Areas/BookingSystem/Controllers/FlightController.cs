using GBC_Travel_Group_136.Data;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
    public class FlightController : Controller
    {
        private readonly AppDbContext _db;

        public FlightController(AppDbContext context)
        {
            _db = context;
        }
		public async Task<IActionResult> Index()
		{
			var flights = await _db.Flights.ToListAsync();
			return View(flights);
		}
	}
}
