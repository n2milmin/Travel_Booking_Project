using GBC_Travel_Group_136.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GBC_Travel_Group_136.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Viewing home page.");

            return View();
        }

        [HttpGet]
        public IActionResult GeneralSearch(string searchType, string searchString)
        {
            _logger.LogInformation($"{searchType} {searchString}");
            if (searchType == "Car")
            {
                return Redirect(Url.Action("Search", "Car", new { area = "BookingSystem", searchString }));
            }
            else if (searchType == "Flight")
            {
                return Redirect(Url.Action("Search", "Flight", new { area = "BookingSystem", searchString }));
            }
            else if (searchType == "Hotel")
            {
                return Redirect(Url.Action("Search", "Hotel", new { area = "BookingSystem", searchString }));
            }
            return RedirectToAction("Index", "Home");
        }

            [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
