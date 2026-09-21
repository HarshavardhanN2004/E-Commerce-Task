using System.Security.Claims;
using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> PlaceOrder(OrderDto orderDto)
    {
        try
        {
            int userId = GetUserId();
            OrderDto order = await _orderService.PlaceOrderAsync(userId,orderDto);
            return Ok(order);
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

    [HttpGet("my-orders")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyOrders()
    {
        try
        {
            int userId = GetUserId();
            List<OrderDto> orders = await _orderService.GetMyOrdersAsync(userId);
            return Ok(orders);
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(exception.Message);
        }
    }

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetById(int orderId)
    {
        try
        {
            OrderDto? order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(order);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred.");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<OrderDto> orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred.");
        }
    }

    [HttpPut("{orderId:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int orderId, string status)
    {
        try
        {
            OrderDto? order = await _orderService.UpdateStatusAsync(orderId,status);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(order);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred.");
        }
    }

    private int GetUserId()
    {
        string? userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdValue,out int userId))
        {
            throw new UnauthorizedAccessException("Invalid user token.");
        }
        return userId;
    }
}