using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Models;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepo;
    private readonly IShoppingCartRepository _cartRepo;
    private readonly UserManager<IdentityUser> _userManager;

    public OrdersController(IOrderRepository orderRepo,
                            IShoppingCartRepository cartRepo,
                            UserManager<IdentityUser> userManager)
    {
        _orderRepo = orderRepo;
        _cartRepo = cartRepo;
        _userManager = userManager;
    }

    // GET /Orders/Checkout
    public IActionResult Checkout()
    {
        var cartItems = _cartRepo.GetCartItems();

        if (cartItems == null || !cartItems.Any())
            return RedirectToAction("Index", "ShoppingCart");

        return View(cartItems);
    }

    // POST /Orders/Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(
     string firstName, string lastName,
     string phone, string address)
    {
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(address))
        {
            ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
            return View(_cartRepo.GetCartItems());
        }

        // ← Lấy email từ tài khoản đăng nhập thay vì từ form
        var user = await _userManager.GetUserAsync(User);
        var userEmail = user?.Email ?? "";

        var cartItems = _cartRepo.GetCartItems();
        var order = new Order
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userEmail,   // ← email từ tài khoản
            Phone = phone,
            Address = address,
            OrderTotal = _cartRepo.GetCartTotal(),
            OrderPlaced = DateTime.Now,
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
        ViewBag.OrderId = orderId;
        return View();
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