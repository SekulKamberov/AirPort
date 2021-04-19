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

        public class SelectListDefaultItem
        {
            public const string All = " -- All -- ";

            public const string SelectTown = " -- Select Town -- ";

            public const string DefaultItemValue = "0";
        }

        public class FieldDisplay
        {
            public const string StartDestination = "Start destination";

            public const string EndDestination = "End destination";

            public const string Company = "Company";

            public const string RememberMe = "Remember me";

            public const string FirstName = "First name";

            public const string LastName = "Last name";

            public const string ConfirmPassword = "Confirm password";

            public const string UniqueReferenceNumber = "Unique reference number";

            public const string ChiefFirstName = "Chief first name";

            public const string ChiefLastName = "Chief last name";

            public const string PhoneNumber = "Phone";
        }

        public class RegexPattern
        {
            public const string Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            public const string CompanyUniqueReferenceNumber = @"^\d{9,13}$";

            public const string Phone = @"^0\d{9}$";

            public const string FriendlyUrl = @"[^A-Za-z0-9_\.~]+";

            public const string Name = @"[a-zA-Z ]+";
        }

        public class Message
        {
            public const string InvalidEmail = "Invalid email address!";

            public const string UsernameLength = "The {0} must be at least {2} and at max {1} characters long.";

            public const string NameContainOnlyLetters = "{0} can contain only letetrs!";

            public const string RegularUserNameMaxLength = "The {0} must be at max {1} symbols long.";

            public const string PasswordLength = "The {0} must be at least {2} and at max {1} characters long.";

            public const string PasswordsMissmatch = "The password and confirmation password do not match.";

            public const string CompanyNameLength = "The {0} must be at least {2} and at max {1} characters long.";

            public const string CompanyDescriptionLength = "The {0} must be between {2} and {1} symbols long.";

            public const string UniqueReferenceNumberFormat = "The {0} must be between 9 and 13 symbols long, containing only digits.";

            public const string CompanyChiefNameMaxLength = "The {0} must be at max {1} symbols long.";

            public const string CompanyAddressLength = "The {0} must be between {2} and {1} symbols long.";

            public const string PhoneNumberFormat = "The {0} should start with '0' containing exactly 10 digits.";

            public const string LogoMaxLength = "The company logo cannot be larger than 500 KB.";

            public const string LogoAvailableFormats = "The company logo can be in the following formats: .jpg, .png or .bmp.";

        }

        public class PictureFormat
        {
            public const string Jpg = ".jpg";

            public const string Png = ".png";

            public const string Bmp = ".bmp";
        }
    }
}
