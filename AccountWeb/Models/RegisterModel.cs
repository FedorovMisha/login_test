using System.ComponentModel.DataAnnotations;

namespace AccountWeb.Models
{
    public class RegisterModel
    {
        [Required]
        [UIHint("email")]
        [EmailAddress(ErrorMessage = "write correct email")]
        public string Email { get; set; }
        
        [Required]
        [UIHint("password")]
        public string Password { get; set; }


        public string ReturnUrl { get; set; } = "/";

    }
}