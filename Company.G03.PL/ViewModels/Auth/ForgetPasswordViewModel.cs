using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Please Enter your Email!!")]
        [EmailAddress(ErrorMessage = "Invalid Email -__-")]
        public string Email { get; set; }
    }
}
