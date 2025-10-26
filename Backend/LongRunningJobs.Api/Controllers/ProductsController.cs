using Microsoft.AspNetCore.Mvc;
using LongRunningJobs.Api.Models;
using LongRunningJobs.Api.Services;

namespace LongRunningJobs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly INorthwindService _northwindService;

    public ProductsController(INorthwindService northwindService)
    {
        _northwindService = northwindService;
    }

    /// <summary>
    /// Search products with pagination and filtering
    /// </summary>
    [HttpPost("search")]
    public async Task<ActionResult<ProductSearchResponse>> SearchProducts([FromBody] ProductSearchRequest request)
    {
        try
        {
            // Validate request
            if (request.Page < 1)
                request.Page = 1;

            if (request.PageSize < 1 || request.PageSize > 50)
                request.PageSize = 10;

            var response = await _northwindService.SearchProductsAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Failed to search products", Details = ex.Message });
        }
    }

    /// <summary>
    /// Get all available product categories
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        try
        {
            var categories = await _northwindService.GetCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Failed to get categories", Details = ex.Message });
        }
    }
}