using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Backend.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required]
    [StringLength(150)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "Product name can contain only letters and spaces.")]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be a whole number greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    [StringLength(500)]
    public string ImagePath { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0.")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}