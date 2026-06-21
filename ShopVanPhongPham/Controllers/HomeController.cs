using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly AppDbContext _context;

    public HomeController(IProductRepository productRepo, AppDbContext context)
    {
        _productRepo = productRepo;
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _productRepo.GetAllProducts(); // ← không await
        return View(products.ToList());
    }

    public IActionResult Contact() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(string fullName, string email, string phone, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        {
            TempData["ErrorMessage"] = "Vui lòng điền đầy đủ thông tin bắt buộc.";
            return RedirectToAction("Contact");
        }

        var contactMessage = new ContactMessage
        {
            FullName = fullName,
            Email = email,
            Phone = phone,
            Subject = string.IsNullOrWhiteSpace(subject) ? "Khác" : subject,
            Message = message,
            SentAt = DateTime.Now,
            IsRead = false
        };

        _context.ContactMessages.Add(contactMessage);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất có thể.";
        return RedirectToAction("Contact");
    }
}