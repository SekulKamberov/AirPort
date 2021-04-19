namespace Airport.Services.Models.Airport
{
    using Common.Automapper;
    using Data.Models;

    public class AirportBaseServiceModel : IMapFrom<Airport>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
