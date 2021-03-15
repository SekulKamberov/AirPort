namespace Airport.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public List<Review> Reviews { get; set; } = new List<Review>();

        public DateTime? RegistrationDate { get; set; }
    }
}
