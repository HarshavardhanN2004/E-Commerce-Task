using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.DTOs;

public class CategoryDto
{
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    [RegularExpression( @"^[A-Za-z ]+$", ErrorMessage = "Category name can contain only characters and spaces.")]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}