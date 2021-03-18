namespace Airport.Common.Constants
{
    public static class WebConstants
    {
        public class DbConnection
        {
            public const string DefaultConnection = "DefaultConnection";
        }

        public class Routing
        {
            public const string HomeError = "/Home/Error";
        }

        public class FilePath
        {
            public const string Towns = "../Airport.Data/SeedData/towns.csv";

            public const string Airports = "../Airport.Data/SeedData/airports.csv";

            public const string Companies = "../Airport.Data/SeedData/companies.csv";

            public const string CompaniesImages = "../Airport.Data/SeedData/Images/Companies/";

            public const string Users = "../Airport.Data/SeedData/users.csv";
        }


    }
}
