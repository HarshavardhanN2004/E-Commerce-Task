using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using E_Commerce_Backend.Services.Interfaces;

namespace E_Commerce_Backend.Services.Implementations;
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository,IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto> GetCartAsync(int userId)
    {
        try
        {
            Cart? cart =await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = 0
                };

                await _cartRepository.AddAsync(cart);
            }

            return MapToDto(cart);
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error in GetCartAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<CartDto> AddItemAsync(int userId,CartItemDto cartItemDto)
    {
        try
        {
            Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = 0
                };
                await _cartRepository.AddAsync(cart);
            }

            Product? product =await _productRepository.GetByIdAsync(cartItemDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            if (cartItemDto.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be at least 1.");
            }
            if (cartItemDto.Quantity > product.Stock)
            {
                throw new InvalidOperationException("Insufficient stock.");
            }

            CartItem? existingItem = cart.CartItems.FirstOrDefault(item =>item.ProductId == cartItemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
                if (existingItem.Quantity > product.Stock)
                {
                    throw new InvalidOperationException("Insufficient stock.");
                }
                existingItem.Subtotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                CartItem newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Quantity = cartItemDto.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = cartItemDto.Quantity * product.Price
                };
                cart.CartItems.Add(newItem);
            }
            RecalculateTotal(cart);
            await _cartRepository.UpdateAsync(cart);
            return MapToDto(cart);
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error in AddItemAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<CartDto?> UpdateItemAsync(int userId,int cartItemId, int quantity)
    {
        try
        {
            Cart? cart =await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return null;
            }
            CartItem? item = cart.CartItems.FirstOrDefault(cartItem =>cartItem.CartItemId == cartItemId);

            if (item == null)
            {
                return null;
            }

            Product? product = await _productRepository.GetByIdAsync(item.ProductId);

            if (product == null)
            {
                return null;
            }
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be at least 1.");
            }

            if (quantity > product.Stock)
            {
                throw new InvalidOperationException("Insufficient stock.");
            }

            item.Quantity = quantity;
            item.Subtotal = quantity * item.UnitPrice;

            RecalculateTotal(cart);
            await _cartRepository.UpdateAsync(cart);
            return MapToDto(cart);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in UpdateItemAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> RemoveItemAsync(int userId,int cartItemId)
    {
        try
        {
            Cart? cart =await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return false;
            }
            CartItem? item =cart.CartItems.FirstOrDefault(cartItem =>cartItem.CartItemId == cartItemId);
            if (item == null)
            {
                return false;
            }

            cart.CartItems.Remove(item);
            RecalculateTotal(cart);
            await _cartRepository.UpdateAsync(cart);
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in RemoveItemAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        try
        {
            Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return false;
            }
            cart.CartItems.Clear();
            cart.TotalAmount = 0;
            await _cartRepository.UpdateAsync(cart);
            return true;
        }
        catch( Exception ex) 
        {
            Console.WriteLine($"Error in ClearCartAsync: {ex.Message}"); 
            throw;
        }
    }

    private static void RecalculateTotal(Cart cart)
    {
        cart.TotalAmount = cart.CartItems.Sum(item => item.Subtotal);
    }

    private static CartDto MapToDto(Cart cart)
    {
        return new CartDto
        {
            CartId = cart.CartId,
            UserId = cart.UserId,
            CreatedAt = cart.CreatedAt,
            TotalAmount = cart.TotalAmount,

            CartItems = cart.CartItems
                .Select(item => new CartItemDto
                {
                    CartItemId = item.CartItemId,
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