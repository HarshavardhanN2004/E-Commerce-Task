using E_Commerce_Backend.Models;
namespace E_Commerce_Backend.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int productId);

    Task<Product> AddAsync(Product product);

    Task<Product> UpdateAsync(Product product);

    Task<bool> DeleteAsync(int productId);
}