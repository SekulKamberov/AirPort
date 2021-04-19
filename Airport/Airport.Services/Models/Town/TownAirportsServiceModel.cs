namespace Airport.Services.Models.Town
{
    using System.Collections.Generic;
    using Models.Airport;

    public class TownAirportsServiceModel : TownBaseServiceModel
    {
        public IEnumerable<AirportBaseServiceModel> Airports { get; set; }
    }
}
