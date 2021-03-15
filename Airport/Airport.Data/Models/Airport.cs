namespace Airport.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class Airport
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.Airport.NameMaxLength)]
        public string Name { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        [Required]
        [MaxLength(DataConstants.Airport.PhoneMaxLength)]
        public string Phone { get; set; }


        public List<Route> ArrivalRoutes { get; set; } = new List<Route>();

        public List<Route> DepartureRoutes { get; set; } = new List<Route>();

    }
}
