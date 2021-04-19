namespace Airport.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using Data;
    using Services.Contracts;
    using Services.Models.Town;

    public class TownService : ITownService
    {
        private readonly AirportDbContext db;

        private readonly IMapper mapper;

        public TownService(AirportDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public string GetTownNameByAirportId(int id)
        {
            var town = this.db.Towns.FirstOrDefault(t => t.Airports.Any(a => a.Id== id));

            if (town != null)
            {
                return town.Name;
            }

            return null;
        }

        public string GetTownNameById(int id)
        {
            var town = this.db.Towns.FirstOrDefault(t => t.Id == id);

            if(town != null)
            {
                return town.Name;
            }

            return null;
        }

        public IEnumerable<TownBaseServiceModel> GetTownsListItems() =>
            this.db.Towns
            .OrderBy(t => t.Name)
            .Select(p => this.mapper.Map<TownBaseServiceModel>(p))
            .ToList();

        public IEnumerable<TownAirportsServiceModel> GetTownsWithAirports() =>
            this.db.Towns
            .OrderBy(t => t.Name)
            .Include(t => t.Airports)
            .Select(p => this.mapper.Map<TownAirportsServiceModel>(p))
            .ToList();

        public bool TownExistsById(int id) =>
            this.db.Towns.Any(t => t.Id == id);
    }
}
