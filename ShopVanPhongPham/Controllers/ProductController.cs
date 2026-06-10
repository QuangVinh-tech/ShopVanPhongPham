using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;

    public ProductController(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    // /Product/Shop
    public IActionResult Shop(string? search, string? category)
    {
        var allProducts = _productRepo.GetAllProducts();

        // Lấy danh sách category cho dropdown
        var categories = allProducts
            .Where(p => !string.IsNullOrEmpty(p.Category))
            .Select(p => p.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        var products = allProducts.AsEnumerable();

        if (!string.IsNullOrEmpty(search))
            products = products
                .Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(category))
            products = products
                .Where(p => p.Category == category);

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Categories = categories;

        return View(products.ToList());
    }

    // /Product/Detail/5
    public IActionResult Detail(int id)
    {
        var product = _productRepo.GetProductById(id);
        if (product == null) return NotFound();
        return View(product);
    }
}