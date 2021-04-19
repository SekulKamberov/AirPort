namespace Airport.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using Airport.Common.Constants;
     
    public class LoginFormModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = WebConstants.FieldDisplay.RememberMe)]
        public bool RememberMe { get; set; }
    }
}
