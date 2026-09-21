using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using E_Commerce_Backend.Services.Interfaces;

namespace E_Commerce_Backend.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        try
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();
            return categories.Select(category => new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            }).ToList();
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}"); 
            throw;
        }
    }

    public async Task<CategoryDto?> GetByIdAsync(int categoryId)
    {
        try
        {
            Category? category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                return null;

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<CategoryDto> CreateAsync(CategoryDto dto)
    {
        try
        {
            Category category = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            Category created = await _categoryRepository.AddAsync(category);

            return new CategoryDto
            {
                CategoryId = created.CategoryId,
                CategoryName = created.CategoryName,
                Description = created.Description
            };
        }
        catch( Exception ex) 
        {
            Console.WriteLine($"Error in CreateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<CategoryDto?> UpdateAsync(int categoryId,CategoryDto dto)
    {
        try
        {
            Category? category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
                return null;

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;

            Category updated = await _categoryRepository.UpdateAsync(category);

            return new CategoryDto
            {
                CategoryId = updated.CategoryId,
                CategoryName = updated.CategoryName,
                Description = updated.Description
            };
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int categoryId)
    {
        try
        {
            return await _categoryRepository.DeleteAsync(categoryId);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }
}