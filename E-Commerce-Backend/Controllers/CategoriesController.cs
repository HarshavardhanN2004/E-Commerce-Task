using E_Commerce_Backend.DTOs;
using E_Commerce_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _categoryService.GetAllAsync());
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            CategoryDto? category =await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound("Category not found.");

            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        try
        {
            CategoryDto category = await _categoryService.CreateAsync(dto);
            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id,CategoryDto dto)
    {
        try
        {
            CategoryDto? category =await _categoryService.UpdateAsync(id, dto);

            if (category == null)
                return NotFound("Category not found.");

            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool deleted = await _categoryService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Category not found.");

            return Ok("Category deleted successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}