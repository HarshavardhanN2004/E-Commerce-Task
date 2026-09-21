using E_Commerce_Backend.Data;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        try
        {
            return await _context.Products
                .Include(product => product.Category)
                .ToListAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Product?> GetByIdAsync(int productId)
    {
        try
        {
            return await _context.Products
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product =>
                    product.ProductId == productId);
        }
        catch( Exception ex ) 
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Product> AddAsync(Product product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        try
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int productId)
    {
        try
        {
            Product? product =await _context.Products.FirstOrDefaultAsync(product =>product.ProductId == productId);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }
}