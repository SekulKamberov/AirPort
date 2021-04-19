namespace Airport.Services.Contracts
{
    using Models.Town;
    using System.Collections.Generic;

    public interface ITownService
    {
        IEnumerable<TownBaseServiceModel> GetTownsListItems();

        IEnumerable<TownAirportsServiceModel> GetTownsWithAirports();

        string GetTownNameByAirportId(int id);

        string GetTownNameById(int id);

        bool TownExistsById(int id);

    }
}
