using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email Is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email !!")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Enter The Password !!")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}
