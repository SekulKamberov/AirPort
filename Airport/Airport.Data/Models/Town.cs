namespace Airport.Data.Models
{  
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class Town
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.Town.TownMaxNameLength)]
        public string Name { get; set; }

        public List<Airport> Airports { get; set; } = new List<Airport>();

        public List<Company> Companies { get; set; } = new List<Company>();
    }
}
