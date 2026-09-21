using E_Commerce_Backend.Data;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Repositories.Implementations;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        try
        {
            return await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .FirstOrDefaultAsync(cart =>
                    cart.UserId == userId);
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"Error in GetCartByUserIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Cart?> GetByIdAsync(int cartId)
    {
        try
        {
            return await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Product)
                .FirstOrDefaultAsync(cart =>
                    cart.CartId == cartId);
        }
        catch( Exception ex ) 
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Cart> AddAsync(Cart cart)
    {
        try
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        try
        {
            await _context.SaveChangesAsync();
            return cart;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in SaveChangesAsync: {ex.Message}");
            throw;
        }
    }
}