using System.ComponentModel.DataAnnotations;

namespace IT15_Trojan_B.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; } // Project ID (GUID)

        public string UserId { get; set; } // Store the aspnetusers ID

        [Required]  // Ensure ProjectName is required
        public string ProjectName { get; set; }

        [Required]  // Ensure JobDescription is required
        public string JobDescription { get; set; }

        [Required]  // Ensure Address is required
        public string Address { get; set; }

        [Required]  // Ensure Deadline is required
        public DateTime Deadline { get; set; }

        public string Status { get; set; } = "Pending";
    }

}
