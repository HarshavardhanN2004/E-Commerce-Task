using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models;

public class CartItem
{
    public int CartItemId { get; set; }

    [Required]
    public int CartId { get; set; }

    [Required]
    public int ProductId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be negative.")]
    public decimal UnitPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Subtotal cannot be negative.")]
    public decimal Subtotal { get; set; }

    public Cart? Cart { get; set; }

    public Product? Product { get; set; }
}