namespace Airport.Services.Models.Town
{
    using Data.Models;
    using Common.Automapper;

    public class TownBaseServiceModel : IMapFrom<Town>
    {
        public int id { get; set; }

        public string Name { get; set; }
    }
}
