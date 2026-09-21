using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<ProductDto> products = await _productService.GetAllAsync();
            return Ok(products);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred while fetching products.");
        }
    }

    [HttpGet("{productId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int productId)
    {
        try
        {
            ProductDto? product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred while fetching the product.");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create( ProductFormDto productFormDto)
    {
        try
        {
            ProductDto product = await _productService.CreateAsync(productFormDto);
            return CreatedAtAction(nameof(GetById),new { productId = product.ProductId },product);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred while creating the product.");
        }
    }


    [HttpPut("{productId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int productId,ProductFormDto productFormDto)
    {
        try
        {
            ProductDto? product = await _productService.UpdateAsync(productId, productFormDto);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(500,"An unexpected error occurred while updating the product.");
        }
    }

    [HttpDelete("{productId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int productId)
    {
        try
        {
            bool deleted =await _productService.DeleteAsync(productId);
            if (!deleted)
            {
                return NotFound("Product not found.");
            }
            return Ok("Product deleted successfully.");
        }
        catch (Exception)
        {
            return StatusCode( 500, "An unexpected error occurred while deleting the product.");
        }
    }
}