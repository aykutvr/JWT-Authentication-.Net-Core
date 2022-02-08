using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class LoginViewModel
    {
        [Display(Name = "E-Mail Address")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
