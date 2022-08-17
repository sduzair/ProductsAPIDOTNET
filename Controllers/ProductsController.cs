using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet]
    public async Task<List<Product>> GetAllOld()
    {
        return await _productsService.GetProductsAsync();
    }

    [HttpGet]
    public async Task<List<Product>> GetAll(int limit = 10, string sort = "id", int sortDirection = 1, int skip = 0, string? category = null)
    {
        if (!(category == null)) return await _productsService.GetProductsByCategoryAsync(category, limit, sort, sortDirection, skip);
        return await _productsService.GetProductsAsync(limit, sort, sortDirection, skip);
    }
    [HttpGet("{id}")]
    public async Task<Product> GetOne(string id)
    {
        return await _productsService.GetProductAsync(id);
    }
    [HttpPost]
    public async Task NewProduct([FromBody] Product p)
    {
        await _productsService.CreateProductAsync(p);
        //return Ok();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(string id, [FromBody] Product updatedProduct)
    {
        var result = await _productsService.UpdateProductAsync(id, updatedProduct);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        var result = await _productsService.RemoveProductAsync(id);
        return Ok(result);
    }
}
