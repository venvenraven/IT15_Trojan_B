using System.ComponentModel.DataAnnotations;

namespace IT15_Trojan_B.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

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
        public string Classification { get; set; } // General Contractor, Subcontractor, etc.

        [Required]
        public string Specialty { get; set; } // Electrical, Plumber, etc.

        public bool IsActive { get; set; } = true; // Default to active employee
        public bool IsAssigned { get; set; } = false; // Default to unassigned

        public string UploadedFilePath { get; set; } // Store file path for uploaded documents
    }
}
