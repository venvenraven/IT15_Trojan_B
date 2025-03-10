using System;
using System.ComponentModel.DataAnnotations;

namespace IT15_Trojan_B.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
