using E_Commerce_Backend.Data;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(user =>user.Email == email);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetByEmailAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(user =>user.UserId == userId);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }
    public async Task<User> AddAsync(User user)
    {
        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch( Exception ex ) 
        {
            Console.WriteLine($"Error in AddAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> UpdateAsync(User user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }
}