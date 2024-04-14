using GBC_Travel_Group_136.Areas.BookingSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_136.Data
{
	public class ContextSeed
	{
		public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Basic.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Guest.ToString()));
		}

		internal static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			var superUser = new ApplicationUser
			{
				UserName = "superAdmin",
				Email = "adminsupport@domain.ca",
				FirstName = "Super",
				LastName = "Admin",
				EmailConfirmed = true,
				PhoneNumberConfirmed = true,
			};

			if (userManager.Users.All(u => u.Id != superUser.Id))
			{
				var user = await userManager.FindByEmailAsync(superUser.Email);
				if (user != null)
				{
					await userManager.CreateAsync(superUser, "Password123!");

					await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
					await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Basic.ToString()));
					await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Guest.ToString()));
				}
			}
		}
	}
}
