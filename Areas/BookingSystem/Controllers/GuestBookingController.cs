using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
	[AllowAnonymous]
	public class GuestBooking : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
