using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Backend.DTOs;

public class CartItemDto
{
    public int CartItemId { get; set; }
    [Required]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue,ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}