using Microsoft.AspNetCore.Mvc;
using ShoppingCartApp.Data;
using ShoppingCartApp.Models.Interfaces;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _cartRepo;
        private readonly AppDbContext _context;

        public ShoppingCartController(IShoppingCartRepository cartRepo, AppDbContext context)
        {
            _cartRepo = cartRepo;
            _context = context;
        }

        // GET: /ShoppingCart/Index
        public IActionResult Index()
        {
            var items = _cartRepo.GetCartItems();
            ViewBag.CartTotal = _cartRepo.GetCartTotal();
            ViewBag.CartCount = _cartRepo.GetCartCount();
            return View(items);
        }

        // POST: /ShoppingCart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return NotFound();

            _cartRepo.AddToCart(product, quantity);
            TempData["SuccessMessage"] = $"Đã thêm \"{product.Name}\" vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        // POST: /ShoppingCart/Remove
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            _cartRepo.RemoveFromCart(productId);
            TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Index");
        }

        // POST: /ShoppingCart/Increase
        [HttpPost]
        public IActionResult Increase(int productId)
        {
            _cartRepo.IncreaseQuantity(productId);
            return RedirectToAction("Index");
        }

        // POST: /ShoppingCart/Decrease
        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            _cartRepo.DecreaseQuantity(productId);
            return RedirectToAction("Index");
        }

        // POST: /ShoppingCart/Clear
        [HttpPost]
        public IActionResult Clear()
        {
            _cartRepo.ClearCart();
            TempData["SuccessMessage"] = "Đã xóa toàn bộ giỏ hàng.";
            return RedirectToAction("Index");
        }
    }
}