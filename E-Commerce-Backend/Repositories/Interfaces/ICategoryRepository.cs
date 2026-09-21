using E_Commerce_Backend.Models;

namespace E_Commerce_Backend.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int categoryId);

    Task<Category> AddAsync(Category category);

    Task<Category> UpdateAsync(Category category);

    Task<bool> DeleteAsync(int categoryId);
}