using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IT15_Trojan_B.Models
{
    public class WorkOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Customer { get; set; }

        [Required]
        public required string JobTitle { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        [Phone]
        public required string ContactNumber { get; set; }

        [Required]
        public required string PriorityStatus { get; set; }

        [Required]
        [Column(TypeName = "date")] // ✅ Store only Date (No Time)
        public DateTime DueDate { get; set; }


        [Required]
        public required string Status { get; set; }
    }
}