using E_Commerce_Backend.Models;
namespace E_Commerce_Backend.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(int userId);

    Task<Cart?> GetByIdAsync(int cartId);

    Task<Cart> AddAsync(Cart cart);

    Task<Cart> UpdateAsync(Cart cart);

    Task SaveChangesAsync();
}