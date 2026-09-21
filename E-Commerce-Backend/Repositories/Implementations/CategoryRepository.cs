using E_Commerce_Backend.Data;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        try
        {
            return await _context.Categories.ToListAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Category?> GetByIdAsync(int categoryId)
    {
        try
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Category> AddAsync(Category category)
    {
        try
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        try
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
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
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
        catch( Exception ex) 
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}"); 
            throw;
        }
    }
}