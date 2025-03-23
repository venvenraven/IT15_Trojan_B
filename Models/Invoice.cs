using System.ComponentModel.DataAnnotations;

namespace IT15_Trojan_B.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string CustomerId { get; set; } // User ID from aspnetusers

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string WorkOrder { get; set; }
        public string ProjectName { get; set; }
        public string AssignedWorker { get; set; }

        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? PaymentDate { get; set; }
    }
}
