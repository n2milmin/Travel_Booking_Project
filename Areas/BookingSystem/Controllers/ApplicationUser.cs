﻿using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_136.Areas.BookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; }
    }
}
