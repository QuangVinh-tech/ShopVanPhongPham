using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _productRepo;

    public HomeController(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public IActionResult Index()
    {
        var products = _productRepo.GetAllProducts(); // ← không await
        return View(products.ToList());
    }

    public IActionResult Contact() => View();
}