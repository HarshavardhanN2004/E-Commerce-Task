using E_Commerce_Backend.DTOs;
namespace E_Commerce_Backend.Services.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync(int userId);
    Task<CartDto> AddItemAsync(int userId,CartItemDto cartItemDto);
    Task<CartDto?> UpdateItemAsync(int userId,int cartItemId,int quantity);
    Task<bool> RemoveItemAsync(int userId,int cartItemId);
    Task<bool> ClearCartAsync(int userId);
}