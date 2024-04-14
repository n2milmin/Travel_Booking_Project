using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_136.Data;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Components.HotelSummary
{
	public class HotelSummaryViewComponent : ViewComponent
	{
		private readonly AppDbContext _db;
		public HotelSummaryViewComponent(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IViewComponentResult> InvokeAsync(int hotelId)
		{
			var hotel = await _db.Hotels
				.Include(x => x.Rooms)
				.SingleOrDefaultAsync(x => x.HotelId == hotelId);

			if (hotel == null)
			{
				return Content("Hotel not found.");
			}

			return View(hotel);
		}
	}
}
