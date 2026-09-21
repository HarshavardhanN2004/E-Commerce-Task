using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Models;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    [RegularExpression( @"^[A-Za-z ]+$", ErrorMessage = "Category name can contain only letters and spaces.")]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}