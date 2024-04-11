using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Mvc;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDbContext _db;

        public CarController(AppDbContext context)
        {
            _db = context;
        }
        public async Task<IActionResult> Index()
        {
            var car = await _db.Cars.ToListAsync();
            return View(car);
        }
    }
}