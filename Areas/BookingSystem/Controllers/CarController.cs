using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Mvc;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GBC_Travel_Group_136.Enum;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
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


		[HttpGet("Book/{carId:int}")]
		public async Task<IActionResult> Book(int carId)
		{
            TempData["CarId"] = carId;

            if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("BookCar", "UserBooking");
			}

			return View("CarBookingOptions");
		}


		[HttpGet("Details/{id:int}")]
		public async Task<IActionResult> Details(int id)
		{
			var car = await _db.Cars.FirstOrDefaultAsync(f => f.CarId == id);

			if (car == null)
			{
				return NotFound();
			}
			return View(car);
		}

		[HttpGet("Create")]
		public IActionResult Create()
		{

			return View();
		}

		[HttpPost("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Car car)
		{
			if (ModelState.IsValid)
			{
				_db.Cars.Add(car);
				await _db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(car);
		}

		[HttpGet("Edit/{id:int}")]
		public async Task<IActionResult> Edit(int id)
		{
			var car = await _db.Cars.FindAsync(id);
			if (car == null)
			{
				return NotFound();
			}
			return View(car);
		}
		[HttpPost("Edit/{id:int}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CarId, Company, Location, Make, Model, Price, Available")] Car car)
		{
			if (id != car.CarId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_db.Update(car);
					await _db.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await FlightExists(car.CarId))
					{
						return NotFound(car);
					}
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(car);
		}

		private async Task<bool> FlightExists(int id)
		{
			return await _db.Cars.AnyAsync(p => p.CarId == id);
		}

		[HttpGet("Delete/{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var car = await _db.Cars.FirstOrDefaultAsync(p => p.CarId == id);
			if (car == null)
			{
				return NotFound();
			}
			return View(car);
		}
		[HttpPost("DeleteConfirmed/{carId:int}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int carId)
		{
			var car = _db.Cars.Find(carId);
			if (car != null)
			{
				_db.Cars.Remove(car);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return NotFound();
		}

		[HttpGet("Search/{searchString?}")]
		public async Task<IActionResult> Search(string searchString)
		{
			var carsQuery = from p in _db.Cars
								select p;

			bool searchPerformed = !string.IsNullOrEmpty(searchString);

			if (searchPerformed)
			{
				carsQuery = carsQuery.Where(f => f.Company.Contains(searchString)
											   || f.Location.Contains(searchString)
											   || f.Make.Contains(searchString)
											   || f.Model.Contains(searchString));
			}

			var car = await carsQuery.ToListAsync();
			ViewData["SearchPerformed"] = searchPerformed;
			ViewData["SearchString"] = searchString;
			return View("Index", car); // Reuse the Index view to display results
		}
	}
}