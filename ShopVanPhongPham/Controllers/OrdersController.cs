using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Models;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepo;
    private readonly IShoppingCartRepository _cartRepo;

    public OrdersController(IOrderRepository orderRepo,
                            IShoppingCartRepository cartRepo)
    {
        _orderRepo = orderRepo;
        _cartRepo = cartRepo;
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
    public IActionResult Checkout(
        string firstName, string lastName,
        string email, string phone,
        string address)
    {
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(address))
        {
            ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
            return View(_cartRepo.GetCartItems());
        }

        var cartItems = _cartRepo.GetCartItems();

        var order = new Order
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
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

        var placedOrder = _orderRepo.PlaceOrder(order); // ← trả về Order có Id
        _cartRepo.ClearCart();

        return RedirectToAction("CheckoutComplete", new { orderId = placedOrder.Id });
    }

    // GET /Orders/CheckoutComplete?orderId=5
    public IActionResult CheckoutComplete(int orderId)
    {
        // Truyền orderId qua ViewBag để hiển thị
        ViewBag.OrderId = orderId;
        return View();
    }
}