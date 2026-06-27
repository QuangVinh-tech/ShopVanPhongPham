using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Helpers;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;

    public ProductController(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public IActionResult Shop(string? search, string? category)
    {
        var allProducts = _productRepo.GetAllProducts();

        var categories = allProducts
            .Where(p => !string.IsNullOrEmpty(p.Category))
            .Select(p => p.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        var products = allProducts.AsEnumerable();

        if (!string.IsNullOrEmpty(search))
        {
            var keyword = StringHelper.RemoveDiacritics(search);   // ← thay đoạn cũ
            products = products.Where(p =>
                StringHelper.RemoveDiacritics(p.Name).Contains(keyword));
        }

        if (!string.IsNullOrEmpty(category))
            products = products.Where(p => p.Category == category);

        ViewBag.Search = search;
        ViewBag.Category = category;
        ViewBag.Categories = categories;

        return View(products.ToList());
    }

    public IActionResult Detail(int id)
    {
        var product = _productRepo.GetProductById(id);
        if (product == null) return NotFound();
        return View(product);
    }
}