using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace E_Commerce_Backend.DTOs;
public class ProductFormDto
{
    [Required]
    [StringLength(150)]
    [RegularExpression( @"^[A-Za-z ]+$",ErrorMessage = "Product name can contain only characters and spaces.")]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public IFormFile? Image { get; set; }
}