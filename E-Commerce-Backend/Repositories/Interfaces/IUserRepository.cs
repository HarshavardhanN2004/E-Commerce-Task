using E_Commerce_Backend.Models;

namespace E_Commerce_Backend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int userId);
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
}