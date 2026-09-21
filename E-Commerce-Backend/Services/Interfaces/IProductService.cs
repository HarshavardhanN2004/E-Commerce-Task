using E_Commerce_Backend.DTOs;

namespace E_Commerce_Backend.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(int productId);

    Task<ProductDto> CreateAsync(ProductFormDto productFormDto);

    Task<ProductDto?> UpdateAsync( int productId, ProductFormDto productFormDto);

    Task<bool> DeleteAsync(int productId);
}