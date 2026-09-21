using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Backend.DTOs;

public class RegisterRequestDto
{
    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "Name can contain only letters and spaces.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}