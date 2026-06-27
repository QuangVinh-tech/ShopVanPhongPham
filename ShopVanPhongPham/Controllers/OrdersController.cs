using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Helpers;
using ShopVanPhongPham.Models;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepo;
    private readonly IShoppingCartRepository _cartRepo;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public OrdersController(IOrderRepository orderRepo,
                            IShoppingCartRepository cartRepo,
                            UserManager<IdentityUser> userManager,
                            IConfiguration config)
    {
        _orderRepo = orderRepo;
        _cartRepo = cartRepo;
        _userManager = userManager;
        _config = config;
    }

    // GET /Orders/Checkout
    [Authorize]
    public IActionResult Checkout()
    {
        var cartItems = _cartRepo.GetCartItems();
        if (cartItems == null || !cartItems.Any())
            return RedirectToAction("Index", "ShoppingCart");

        // ← THÊM MỚI: build sẵn QR preview để hiện ngay khi khách chọn "QR" ở trang Checkout
        var total = _cartRepo.GetCartTotal();
        var bank = _config.GetSection("BankInfo");
        ViewBag.QrPreviewUrl = VietQrHelper.BuildQrUrl(
            bank["BankId"]!, bank["AccountNo"]!, bank["AccountName"]!,
            total, "Thanh toan VPP Shop");

        return View(cartItems);
    }

    // POST /Orders/Checkout
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(
     string firstName, string lastName,
     string phone, string address,
     string paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(address))
        {
            ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
            return View(_cartRepo.GetCartItems());
        }

        var user = await _userManager.GetUserAsync(User);
        var userEmail = user?.Email ?? "";

        var cartItems = _cartRepo.GetCartItems();
        var order = new Order
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userEmail,
            Phone = phone,
            Address = address,
            OrderTotal = _cartRepo.GetCartTotal(),
            OrderPlaced = DateTime.Now,
            PaymentMethod = string.IsNullOrWhiteSpace(paymentMethod) ? "COD" : paymentMethod,
            PaymentStatus = "Chưa thanh toán",
            OrderDetails = cartItems.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product!.Price
            }).ToList()
        };

        var placedOrder = _orderRepo.PlaceOrder(order);
        _cartRepo.ClearCart();
        HttpContext.Session.SetInt32("CartCount", 0);

        return RedirectToAction("CheckoutComplete", new { orderId = placedOrder.Id });
    }

    // GET /Orders/CheckoutComplete
    public IActionResult CheckoutComplete(int orderId)
    {
        var order = _orderRepo.GetOrderById(orderId);
        if (order == null) return RedirectToAction("Index", "Home");

        ViewBag.OrderId = orderId;

        if (order.PaymentMethod == "QR")
        {
            var bank = _config.GetSection("BankInfo");
            ViewBag.QrUrl = VietQrHelper.BuildQrUrl(
                bank["BankId"]!, bank["AccountNo"]!, bank["AccountName"]!,
                order.OrderTotal, $"DH{order.Id}");
        }

        return View(order);
    }

    // GET /Orders/MyOrders
    [Authorize]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        var orders = _orderRepo.GetAllOrders()
                               .Where(o => o.Email == user!.Email)
                               .OrderByDescending(o => o.OrderPlaced)
                               .ToList();
        return View(orders);
    }
}