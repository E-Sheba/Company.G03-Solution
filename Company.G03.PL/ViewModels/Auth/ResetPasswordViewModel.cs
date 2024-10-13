using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please Enter The Password !!")]
        [DataType(DataType.Password)]



        public string Password { get; set; }
        [Required(ErrorMessage = "Please Confirm The Password !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
