using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using E_Commerce_Backend.Services.Interfaces;

namespace E_Commerce_Backend.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository,ICartRepository cartRepository,IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderDto> PlaceOrderAsync(int userId,OrderDto orderDto)
    {
        try
        {
            Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            Order order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",

                Name = orderDto.Name,
                Address = orderDto.Address,
                City = orderDto.City,
                State = orderDto.State,
                PostalCode = orderDto.PostalCode,
                PhoneNumber = orderDto.PhoneNumber,
                PaymentMethod = orderDto.PaymentMethod,

                ShippingAmount = 0
            };

            foreach (CartItem cartItem in cart.CartItems)
            {
                Product? product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found.");
                }
                if (cartItem.Quantity > product.Stock)
                {
                    throw new InvalidOperationException($"Insufficient stock for {product.ProductName}.");
                }

                OrderItem orderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = cartItem.Quantity * product.Price
                };
                order.OrderItems.Add(orderItem);
                product.Stock -= cartItem.Quantity;
                await _productRepository.UpdateAsync(product);
            }
            order.TotalAmount = order.OrderItems.Sum(item => item.Subtotal);
            order.GrandTotal = order.TotalAmount + order.ShippingAmount;
            Order createdOrder =  await _orderRepository.AddAsync(order);
            cart.CartItems.Clear();
            cart.TotalAmount = 0;
            await _cartRepository.UpdateAsync(cart);
            return MapToDto(createdOrder);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error PlaceOrderAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<List<OrderDto>> GetMyOrdersAsync(int userId)
    {
        try
        {
            List<Order> orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(MapToDto).ToList();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error GetMyOrdersAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<OrderDto?> GetByIdAsync(int orderId)
    {
        try
        {
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }
            return MapToDto(order);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        try
        {
            List<Order> orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto).ToList();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<OrderDto?> UpdateStatusAsync(int orderId,string status)
    {
        try
        {
            Order? order =await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }
            string[] validStatuses = {"Pending","Confirmed","Shipped","Delivered","Cancelled" };
            if (!validStatuses.Contains(status))
            {
                throw new ArgumentException("Invalid order status.");
            }

            order.Status = status;
            Order updatedOrder = await _orderRepository.UpdateAsync(order);
            return MapToDto(updatedOrder);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error UpdateStatusAsync: {ex.Message}");
            throw;
        }
       
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            OrderId = order.OrderId,
            UserId = order.UserId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            ShippingAmount = order.ShippingAmount,
            GrandTotal = order.GrandTotal,
            Status = order.Status,

            Name = order.Name,
            Address = order.Address,
            City = order.City,
            State = order.State,
            PostalCode = order.PostalCode,
            PhoneNumber = order.PhoneNumber,
            PaymentMethod = order.PaymentMethod,

            OrderItems = order.OrderItems
                .Select(item => new OrderItemDto
                {
                    OrderItemId = item.OrderItemId,
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName ?? string.Empty,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = item.Subtotal
                })
                .ToList()
        };
    }
}