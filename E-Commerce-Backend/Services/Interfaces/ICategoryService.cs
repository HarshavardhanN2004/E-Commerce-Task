using E_Commerce_Backend.DTOs;

namespace E_Commerce_Backend.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(int categoryId);

    Task<CategoryDto> CreateAsync(CategoryDto categoryDto);

    Task<CategoryDto?> UpdateAsync(int categoryId,CategoryDto categoryDto);

    Task<bool> DeleteAsync(int categoryId);
}