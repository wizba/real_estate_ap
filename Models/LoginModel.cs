using System.ComponentModel.DataAnnotations;

namespace real_estate_api.Models
{
    public class LoginModel
    {
        [Required]  // This is a data annotation
        [EmailAddress]  // This validation ensures it's a valid email format
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
