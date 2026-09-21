using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Models;
using E_Commerce_Backend.Repositories.Interfaces;
using E_Commerce_Backend.Services.Interfaces;

namespace E_Commerce_Backend.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _environment;

    public ProductService(IProductRepository productRepository,IWebHostEnvironment environment)
    {
        _productRepository = productRepository;
        _environment = environment;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        try
        {
            List<Product> products = await _productRepository.GetAllAsync();

            return products.Select(product => new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImagePath = product.ImagePath,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? string.Empty
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<ProductDto?> GetByIdAsync(int productId)
    {
        try
        {
            Product? product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImagePath = product.ImagePath,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<ProductDto> CreateAsync(ProductFormDto productFormDto)
    {
        try
        {
            string imagePath = string.Empty;
            if (productFormDto.Image != null)
            {
                imagePath = await SaveImageAsync(productFormDto.Image);
            }
            Product product = new Product
            {
                ProductName = productFormDto.ProductName,
                Description = productFormDto.Description,
                Price = productFormDto.Price,
                Stock = productFormDto.Stock,
                ImagePath = imagePath,
                CategoryId = productFormDto.CategoryId
            };

            Product createdProduct = await _productRepository.AddAsync(product);
            return new ProductDto
            {
                ProductId = createdProduct.ProductId,
                ProductName = createdProduct.ProductName,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                Stock = createdProduct.Stock,
                ImagePath = createdProduct.ImagePath,
                CategoryId = createdProduct.CategoryId,
                CategoryName = createdProduct.Category?.CategoryName?? string.Empty
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine( $"Error in CreateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<ProductDto?> UpdateAsync(int productId,ProductFormDto productFormDto)
    {
        try
        {
            Product? existingProduct = await _productRepository.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.ProductName = productFormDto.ProductName;
            existingProduct.Description = productFormDto.Description;
            existingProduct.Price = productFormDto.Price;
            existingProduct.Stock = productFormDto.Stock;
            existingProduct.CategoryId = productFormDto.CategoryId;
            if (productFormDto.Image != null)
            {
                existingProduct.ImagePath = await SaveImageAsync(productFormDto.Image);
            }
            Product updatedProduct = await _productRepository.UpdateAsync(existingProduct);

            return new ProductDto
            {
                ProductId = updatedProduct.ProductId,
                ProductName = updatedProduct.ProductName,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                Stock = updatedProduct.Stock,
                ImagePath = updatedProduct.ImagePath,
                CategoryId = updatedProduct.CategoryId,
                CategoryName = updatedProduct.Category?.CategoryName?? string.Empty
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int productId)
    {
        try
        {
            return await _productRepository.DeleteAsync(productId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
            throw;
        }
    }

    private async Task<string> SaveImageAsync(IFormFile image)
    {
        string folderPath = Path.Combine(_environment.WebRootPath,"images","products");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string extension = Path.GetExtension(image.FileName);
        string fileName =$"{Guid.NewGuid()}{extension}";
        string filePath = Path.Combine(folderPath, fileName);
        using (FileStream stream = new FileStream(filePath,FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return $"/images/products/{fileName}";
    }
}