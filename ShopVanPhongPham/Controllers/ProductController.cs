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
        var products = _productRepo.GetAllProducts(); // ← không await

        if (!string.IsNullOrEmpty(search))
            products = products
                .Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(category))
            products = products
                .Where(p => p.Category == category);

        ViewBag.Search = search;
        ViewBag.Category = category;
        return View(products.ToList());
    }

    // /Product/Detail/5
    public IActionResult Detail(int id)
    {
        var product = _productRepo.GetProductById(id); // ← không await
        if (product == null) return NotFound();
        return View(product);
    }
}