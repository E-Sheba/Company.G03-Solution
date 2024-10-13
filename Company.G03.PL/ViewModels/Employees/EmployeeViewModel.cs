using Company.G03.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Company.G03.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Range(25, 60, ErrorMessage = "Age Must Be Between 25 : 60")]
        public int Age { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{3,10}$"
                            , ErrorMessage = "Address Must be Like '123-Street-City-Country'")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Salary is Required")]
        [DataType(DataType.Currency)]
        public double Salary { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [EmailAddress]
        public string? EmailConfirmation { get; set; }
        public bool IsActive { get; set; }
        public string? IsDeleted { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public int? WorkForId { get; set; } //fk by convention
        public Department? WorkFor { get; set; } // optional
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }

    }
}
