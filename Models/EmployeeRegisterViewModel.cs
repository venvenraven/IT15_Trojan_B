using System.ComponentModel.DataAnnotations;

namespace IT15_Trojan_B.ViewModels
{
    public class EmployeeRegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Classification { get; set; } // General Contractor, Subcontractor, etc.

        [Required]
        public string Specialty { get; set; } // Electrical, Plumber, etc.

        [Required(ErrorMessage = "Please upload a file.")]
        public IFormFile UploadFile { get; set; }
    }
}
