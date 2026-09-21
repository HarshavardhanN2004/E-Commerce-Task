using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register( RegisterRequestDto registerRequest)
    {
        try
        {
            bool registered =await _authService.RegisterAsync(registerRequest);
            if (!registered)
            {
                return BadRequest("Email already exists.");
            }
            return Ok("Registration successful.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login( LoginRequestDto loginRequest)
    {
        try
        {
            LoginResponseDto? response = await _authService.LoginAsync(loginRequest);
            if (response == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized("Invalid user token.");
            }
            var user = await _authService.GetProfileAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(new{user.UserId,user.Name,user.Email,user.Role});
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile( UpdateProfileDto updateProfileDto)
    {
        try
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue,out int userId))
            {
                return Unauthorized("Invalid user token.");
            }
            var updatedUser = await _authService.UpdateProfileAsync(userId,updateProfileDto);
            if (updatedUser == null)
            {
                return NotFound("User not found.");
            }
            return Ok(new
            {
                updatedUser.UserId,
                updatedUser.Name,
                updatedUser.Email,
                updatedUser.Role,
                updatedUser.Address,
                updatedUser.City,
                updatedUser.State,
                updatedUser.PostalCode,
                updatedUser.PhoneNumber
            });
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred while updating profile.");
        }
    }
}