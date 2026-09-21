using E_Commerce_Backend.Models;

namespace E_Commerce_Backend.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);

    Task<List<Order>> GetByUserIdAsync(int userId);

    Task<Order?> GetByIdAsync(int orderId);

    Task<List<Order>> GetAllAsync();

    Task<Order> UpdateAsync(Order order);
}