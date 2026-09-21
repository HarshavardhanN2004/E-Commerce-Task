using System.Security.Claims;
using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Customer")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        try
        {
            int userId = GetUserId();
            CartDto cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(exception.Message);
        }
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem( CartItemDto cartItemDto)
    {
        try
        {
            int userId = GetUserId();
            CartDto cart = await _cartService.AddItemAsync(userId,cartItemDto);
            return Ok(cart);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
    [HttpPut("items/{cartItemId:int}")]
    public async Task<IActionResult> UpdateItem(int cartItemId,[FromBody] int quantity)
    {
        try
        {
            int userId = GetUserId();
            CartDto? cart = await _cartService.UpdateItemAsync(userId,cartItemId,quantity);
            if (cart == null)
            {
                return NotFound("Cart item not found.");
            }

            return Ok(cart);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(exception.Message);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("items/{cartItemId:int}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        try
        {
            int userId = GetUserId();
            bool removed = await _cartService.RemoveItemAsync(userId,cartItemId);
            if (!removed)
            {
                return NotFound("Cart item not found.");
            }
            return Ok("Item removed successfully.");
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(exception.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        try
        {
            int userId = GetUserId();
            bool cleared = await _cartService.ClearCartAsync(userId);
            if (!cleared)
            {
                return NotFound("Cart not found.");
            }
            return Ok("Cart cleared successfully.");
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(exception.Message);
        }
    }
    private int GetUserId()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user token.");
        }
        return userId;
    }
}