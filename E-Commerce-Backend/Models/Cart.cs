using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models;

public class Cart
{
    public int CartId { get; set; }

    [Required]
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Range(0, double.MaxValue, ErrorMessage = "Total amount cannot be negative.")]
    public decimal TotalAmount { get; set; }

    public User? User { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}