using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Backend.Models;

public class Order
{
    public int OrderId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
    public int UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "TotalAmount cannot be negative.")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "ShippingAmount cannot be negative.")]
    public decimal ShippingAmount { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "GrandTotal cannot be negative.")]
    public decimal GrandTotal { get; set; }

    [Required]
    [RegularExpression(@"^(Pending|Confirmed|Shipped|Delivered|Cancelled)$",ErrorMessage = "Invalid order status.")]
    public string Status { get; set; } = "Pending";

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Name can contain only letters and spaces.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "City can contain only letters and spaces.")]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "State can contain only letters and spaces.")]
    public string State { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{6}$",ErrorMessage = "PostalCode must contain exactly 6 digits.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    [RegularExpression( @"^[6-9]\d{9}$",ErrorMessage = "PhoneNumber must contain exactly 10 digits and start with 6, 7, 8, or 9.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(Cash on Delivery|Mock Payment)$", ErrorMessage = "Invalid payment method.")]
    public string PaymentMethod { get; set; } = "Cash on Delivery";

    public User? User { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}