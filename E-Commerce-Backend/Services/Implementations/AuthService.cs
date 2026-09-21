using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using E_Commerce_Backend.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce_Backend.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository,IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto registerRequest)
    {
        try
        {
            User? existingUser = await _userRepository.GetByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                return false;
            }
            User user = new User
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                Role = "Customer"
            };

            await _userRepository.AddAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        try
        {
            User? user = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return null;
            }
            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password,user.PasswordHash);
            if (!passwordValid)
            {
                return null;
            }

            string token = GenerateToken(user);
            return new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LoginAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> GetProfileAsync(int userId)
    {
        try
        {
            return await _userRepository.GetByIdAsync(userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetProfileAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<User?> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto)
    {
        try
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            user.Address = updateProfileDto.Address;
            user.City = updateProfileDto.City;
            user.State = updateProfileDto.State;
            user.PostalCode = updateProfileDto.PostalCode;
            user.PhoneNumber = updateProfileDto.PhoneNumber;

            return await _userRepository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateProfileAsync: {ex.Message}");
            throw;
        }
    }
    private string GenerateToken(User user)
    {
        try
        {
            string? key = _configuration["Jwt:Key"];
            string? issuer = _configuration["Jwt:Issuer"];
            string? audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            SymmetricSecurityKey securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            SigningCredentials credentials =new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            [
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            ];

            JwtSecurityToken token = new JwtSecurityToken(issuer: issuer,audience: audience,claims: claims,expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: credentials);
            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GenerateToken: {ex.Message}");
            throw;
        }
    }
}