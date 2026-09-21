using E_Commerce_Backend.Data;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Repositories.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order> AddAsync(Order order)
    {
        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        catch(Exception ex) 
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Order>> GetByUserIdAsync(int userId)
    {
        try
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .Where(order => order.UserId == userId)
                .OrderByDescending(order => order.OrderDate)
                .ToListAsync();
        }
        catch( Exception ex ) 
        {
            Console.WriteLine($"Error in GetByUserIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        try
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .FirstOrDefaultAsync(order =>
                    order.OrderId == orderId);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Order>> GetAllAsync()
    {
        try
        {
            return await _context.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .OrderByDescending(order => order.OrderDate)
                .ToListAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}