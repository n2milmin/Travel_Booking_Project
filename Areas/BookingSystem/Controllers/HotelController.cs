using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
    public class HotelController : Controller
    {
        private readonly AppDbContext _db;

        public HotelController(AppDbContext context)
        {
            _db = context;
        }
		public async Task<IActionResult> Index()
		{
			var hotel = await _db.Hotels.ToListAsync();
			return View(hotel);
		}
	}
}
