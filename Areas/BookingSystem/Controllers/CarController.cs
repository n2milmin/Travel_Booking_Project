using GBC_Travel_Group_136.Data;
using Microsoft.AspNetCore.Mvc;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GBC_Travel_Group_136.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using GBC_Travel_Group_136.Areas.Identity.Pages.Account;
using GBC_Travel_Group_136.Filters;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Controllers
{
	[Area("BookingSystem")]
	[Route("[area]/[controller]/[action]")]
    public class CarController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CarController> _logger;

        public CarController(AppDbContext context, ILogger<CarController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Viewing Cars list");

            var car = await _db.Cars.ToListAsync();
            return View(car);
        }


		[HttpGet("Book/{carId:int}")]
		public IActionResult Book(int carId)
		{
            _logger.LogInformation($"Starting booking a car with id: {carId}");

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
            _logger.LogInformation($"Viewing details of Car with id: {id}");

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
            _logger.LogInformation($"Creating a car");

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

                _logger.LogInformation($"Finished creating car, new id: {car.CarId}");

                return RedirectToAction("Index");
			}
			return View(car);
		}

		[HttpGet("Edit/{id:int}")]
		public async Task<IActionResult> Edit(int id)
		{
            _logger.LogInformation($"Started editing car with id: {id}");

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
            _logger.LogInformation($"Finished editing car with id: {id}");

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
					if (!await CarExists(car.CarId))
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

		private async Task<bool> CarExists(int id)
		{
			return await _db.Cars.AnyAsync(p => p.CarId == id);
		}

		[HttpGet("Delete/{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
            _logger.LogInformation($"starting delete process for car with id: {id}");

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
            _logger.LogInformation($"Deleting car with id: {carId}");

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
            _logger.LogInformation($"User searched for: {searchString}");

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