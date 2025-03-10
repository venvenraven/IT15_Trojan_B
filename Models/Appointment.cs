using System;
using System.ComponentModel.DataAnnotations;

namespace IT15_Trojan_B.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Status { get; set; } // Example: "Scheduled", "Completed"
    }
}
