using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public string Status { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "Name can contain only letters and spaces.")]
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
    [RegularExpression(@"^[6-9]\d{9}$",ErrorMessage = "PhoneNumber must contain exactly 10 digits and start with 6, 7, 8, or 9.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(Cash on Delivery|Mock Payment)$",ErrorMessage = "Invalid payment method.")]
    public string PaymentMethod { get; set; } = "Cash on Delivery";

    public List<OrderItemDto> OrderItems { get; set; }
        = new List<OrderItemDto>();
}

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}