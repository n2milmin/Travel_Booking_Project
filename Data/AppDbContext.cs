using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_136.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
		public DbSet<Booking> Bookings { get; set; }



		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.HasDefaultSchema("Identity");

			builder.Entity<Car>().HasData(
				new Car
				{
					CarId = 1,
					Company = "Steve's Rentals",
					Location = "Toronto",
					Make = "Ford",
					Model = "F-150",
					Price = 50,
					Available = true
				});
			builder.Entity<Car>().HasData(
				new Car
				{
					CarId = 2,
					Company = "Rockey Rentals",
					Location = "Vancouver",
					Make = "Mazda",
					Model = "CX-5",
					Price = 50,
					Available = true
				});
			builder.Entity<Flight>().HasData(
				new Flight
				{
					FlightId = 1,
					Airline = "Air Canada",
					Origin = "Toronto",
					Destination = "Vancouver",
					Departure = DateTime.Now,
					Arrival = DateTime.Now
				});
			builder.Entity<Flight>().HasData(
				new Flight
				{
					FlightId = 2,
					Airline = "Air Canada",
					Origin = "Vancouver",
					Destination = "Toronto",
					Departure = DateTime.Now,
					Arrival = DateTime.Now
				});
			builder.Entity<Seat>().HasData(
				new Seat
				{
					SeatId = 1,
					SeatType = "First Class",
					Amount = 15,
					Price = 500,
					FlightId = 1
				});
			builder.Entity<Seat>().HasData(
				new Seat
				{
					SeatId = 2,
					SeatType = "Buisness",
					Amount = 35,
					Price = 300,
					FlightId = 1
				});
			builder.Entity<Seat>().HasData(
				new Seat
				{
					SeatId = 3,
					SeatType = "Economy",
					Amount = 100,
					Price = 150,
					FlightId = 1
				});
			builder.Entity<Seat>().HasData(
				new Seat
				{
					SeatId = 4,
					SeatType = "First Class",
					Amount = 15,
					Price = 500,
					FlightId = 2
				});
			builder.Entity<Seat>().HasData(
			new Seat
			{
				SeatId = 5,
				SeatType = "Buisness",
				Amount = 35,
				Price = 300,
				FlightId = 2
			});
			builder.Entity<Seat>().HasData(
			new Seat
			{
				SeatId = 6,
				SeatType = "Economy",
				Amount = 100,
				Price = 150,
				FlightId = 2
			});
			builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    HotelId = 1,
                    HotelName = "Best Western",
                    Location = "Toronto",
                    Description = "Pretty"
                });
			builder.Entity<Hotel>().HasData(
				new Hotel
				{
					HotelId = 2,
					HotelName = "Best Western",
					Location = "Vancouver",
					Description = "Pretty"
				});
			builder.Entity<Room>().HasData(
				new Room
				{
					RoomId = 1,
					RoomType = "Suit",
					Description = "King bed, kitchen, tv, etc",
					Amount = 15,
					Price = 500,
					HotelId = 1
				});
			builder.Entity<Room>().HasData(
				new Room
				{
					RoomId = 2,
					RoomType = "Sinlge",
					Description = "Twin bed, window, Tv, mini fridge",
					Amount = 15,
					Price = 100,
					HotelId = 1
				});
			builder.Entity<Room>().HasData(
			new Room
			{
				RoomId = 3,
				RoomType = "Suit",
				Description = "King bed, kitchen, tv, etc",
				Amount = 15,
				Price = 500,
				HotelId = 2
			});
			builder.Entity<Room>().HasData(
			new Room
			{
				RoomId = 4,
				RoomType = "Sinlge",
				Description = "Twin bed, window, Tv, mini fridge",
				Amount = 15,
				Price = 100,
				HotelId = 2
			});


            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "UserToken");
            });
        }
    }
}