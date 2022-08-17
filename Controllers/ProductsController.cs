﻿using Microsoft.AspNetCore.Mvc;
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
    //[HttpGet("{category}")]
    //public async Task<List<Product>> GetByCategory(string category, int limit = 10, string sort = "id", int sortDirection = 1, int skip = 0)
    //{
    //    return await _productsService.GetProductsByCategoryAsync(category, limit, sort, sortDirection, skip);
    //}
    //create cart
    //add product to cart
}
