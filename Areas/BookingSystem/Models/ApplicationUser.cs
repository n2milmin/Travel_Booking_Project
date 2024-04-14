using GBC_Travel_Group_136.Enum;
using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        //public Roles UserRole { get; set; } = Roles.Basic;
        public string FirstName { get; set; }
        public string LastName { get; set; }
		public int UsernameChangeLimit { get; set; }

	}
}
