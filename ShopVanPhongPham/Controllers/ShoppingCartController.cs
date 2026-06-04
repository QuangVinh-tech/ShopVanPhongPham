using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _cart;
        private readonly AppDbContext _context;

        public ShoppingCartController(IShoppingCartRepository cart, AppDbContext context)
        {
            _cart = cart;
            _context = context;
        }

        public IActionResult Index()
        {
            var items = _cart.GetCartItems();
            ViewBag.CartTotal = _cart.GetCartTotal();
            ViewBag.CartCount = _cart.GetCartCount();
            return View(items);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return NotFound();
            _cart.AddToCart(product, quantity);
            TempData["SuccessMessage"] = $"Đã thêm \"{product.Name}\" vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _cart.RemoveFromCart(id);
            TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Increase(int productId)
        {
            _cart.IncreaseQuantity(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            _cart.DecreaseQuantity(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _cart.ClearCart();
            TempData["SuccessMessage"] = "Đã xóa toàn bộ giỏ hàng.";
            return RedirectToAction("Index");
        }
    }
}