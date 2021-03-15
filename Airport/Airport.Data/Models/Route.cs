namespace Airport.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Constants;
    using Data.Enums;

    public class Route
    {
        public int Id { get; set; }

        public int StartAirportId { get; set; }

        public Airport StartAirport { get; set; }

        public int EndAirportId { get; set; }

        public Airport EndAirport { get; set; }

        [Range(typeof(TimeSpan), DataConstants.Route.DepartureTimeMinValue, DataConstants.Route.DepartureTimeMaxValue)]
        public TimeSpan DepartureTime { get; set; }

        [Range(typeof(TimeSpan), DataConstants.Route.DurationMinValue, DataConstants.Route.DurationMaxValue)]
        public TimeSpan Duration { get; set; }

        [Required]
        public AircraftType AircraftType { get; set; }

        [Range(DataConstants.Route.PriceMinValue, DataConstants.Route.PriceMaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public bool IsActive { get; set; }

        public List<Ticket> Tickets { get; set; } 
    }
}
