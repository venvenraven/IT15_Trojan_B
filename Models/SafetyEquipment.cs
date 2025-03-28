﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IT15_Trojan_B.Models
{
    public class SafetyEquipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        public required string Brand { get; set; }

        public required string Supplier { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public required string Condition { get; set; }

        [Required]
        public DateTime DateAcquired { get; set; }
    }

}