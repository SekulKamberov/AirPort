namespace Airport.Common.Constants
{
    public static class DataConstants
    {
        public class User
        {
            public const int NameMaxLength = 35;

            public const int PasswordMinLength = 3;
        }

        public class Review
        {
            public const int DescriptionMinLength = 20;
            public const int DescriptionMaxLength = 150;
        }

        public class Company
        {
            public const int NameMinLength = 20;
            public const int NameMaxLength = 150;

            public const int LogoMaxLength = 500 * 1024;

            public const int DescriptionMinLength = 500 * 1024;
            public const int DescriptionMaxLength = 3000;

            public const int UniqueReferenceNumberMinLength = 9;
            public const int UniqueReferenceNumberMaxLength = 13;

            public const int ChiefFirstNameMaxLength = 20;
            public const int ChiefLastNameMaxLength = 20;

            public const int AddressMinLength = 10;
            public const int AddressMaxLength = 95;
        }

        public class Town
        {
            public const int TownMaxNameLength = 50;
        }

        public class Airport
        {
            public const int NameMaxLength = 10;
            public const int PhoneMaxLength = 95;
        }

        public class Route
        {
            public const string DepartureTimeMinValue = "00:00";
            public const string DepartureTimeMaxValue = "23:59";

            public const string DurationMinValue = "00:00";
            public const string DurationMaxValue = "24:00";

            public const int PriceMinValue = 250;
            public const int PriceMaxValue = 11000;
        }

        public class Ticket
        {
            public const int SeatMinValue = 1;
            public const int SeatMaxValue = 45;
        }
         

    }
}
