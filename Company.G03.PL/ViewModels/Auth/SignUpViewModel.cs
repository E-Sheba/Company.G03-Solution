using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage = "Please Enter the UserName!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter your FirstName!!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter your LastName!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter your Email!!")]
        [EmailAddress(ErrorMessage ="Invalid Email -__-")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter your Password!!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter your ConfirmPassword!!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Don't Match!!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must Agree!!")]
        public bool IsAgree { get; set; }
    }
}
