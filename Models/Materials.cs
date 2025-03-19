using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IT15_Trojan_B.Models;

public class Material
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public required string Unit { get; set; }

    public required string Brand { get; set; }
    public required string Supplier { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public required string Status { get; set; }
    public required string StorageLocation { get; set; }

    [Required]
    [Column(TypeName = "date")] // ✅ Store only Date (No Time)
    public DateTime DateAcquired { get; set; }

}