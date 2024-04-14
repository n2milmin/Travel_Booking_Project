using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_136.Data;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Components.FlightSummary
{
	public class FlightSummaryViewComponent : ViewComponent
	{
		private readonly AppDbContext _db;
		public FlightSummaryViewComponent(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IViewComponentResult> InvokeAsync(int flightId)
		{
			var flight = await _db.Flights
				.Include(x => x.Seats)
				.SingleOrDefaultAsync(x => x.FlightId == flightId);

			if (flight == null)
			{
				return Content("Flight not found.");
			}

			return View(flight);
		}
	}
}
