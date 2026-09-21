using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Models;

namespace E_Commerce_Backend.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequestDto registerRequest);

    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);

    Task<User?> GetProfileAsync(int userId);

    Task<User?> UpdateProfileAsync(int userId,UpdateProfileDto updateProfileDto);
}