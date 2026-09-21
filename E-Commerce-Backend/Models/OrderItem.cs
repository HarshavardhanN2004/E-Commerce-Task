using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Backend.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
    public int OrderId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "UnitPrice cannot be negative.")]
    public decimal UnitPrice { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Subtotal cannot be negative.")]
    public decimal Subtotal { get; set; }

    public Order? Order { get; set; }

    public Product? Product { get; set; }
}