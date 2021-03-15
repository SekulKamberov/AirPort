namespace Airport.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;
    using Data.Enums;

    public class RegularUser : User
    {
        [Required]
        [MaxLength(DataConstants.User.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DataConstants.User.NameMaxLength)]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
