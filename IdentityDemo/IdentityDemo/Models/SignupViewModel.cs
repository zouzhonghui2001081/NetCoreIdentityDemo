using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Models
{
    public class SignupViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email Address is invalid")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password is invalid")]
        public string Password { get; set; }
    }
}
