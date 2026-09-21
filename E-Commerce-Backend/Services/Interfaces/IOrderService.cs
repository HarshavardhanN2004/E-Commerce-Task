using E_Commerce_Backend.DTOs;

namespace E_Commerce_Backend.Services.Interfaces;
public interface IOrderService
{
    Task<OrderDto> PlaceOrderAsync(int userId,OrderDto orderDto);
    Task<List<OrderDto>> GetMyOrdersAsync(int userId);
    Task<OrderDto?> GetByIdAsync(int orderId);
    Task<List<OrderDto>> GetAllAsync();

    Task<OrderDto?> UpdateStatusAsync(int orderId,string status);
}