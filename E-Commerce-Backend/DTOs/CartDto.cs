namespace E_Commerce_Backend.DTOs;
public class CartDto
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CartItemDto> CartItems { get; set; }
        = new List<CartItemDto>();
}