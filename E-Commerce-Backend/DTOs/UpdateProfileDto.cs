using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Backend.DTOs;

public class UpdateProfileDto
{
    [StringLength(250)]
    public string? Address { get; set; }

    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "City can contain only letters and spaces.")]
    public string? City { get; set; }

    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "State can contain only letters and spaces.")]
    public string? State { get; set; }

    [RegularExpression(@"^\d{6}$",ErrorMessage = "Postal code must contain exactly 6 digits.")]
    public string? PostalCode { get; set; }

    [RegularExpression(@"^[6-9]\d{9}$",ErrorMessage = "Phone number must be a valid 10 digit number starting with 6-9.")]
    public string? PhoneNumber { get; set; }
}