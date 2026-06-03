using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models.Interfaces;
using ShopVanPhongPham.Models.Services;

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

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var items = _cart.GetCartItems();
            ViewBag.CartTotal = _cart.GetTotal();
            return View(items);
        }

        // Thêm sản phẩm vào giỏ
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _cart.AddToCart(product);
            }
            return RedirectToAction("Index");
        }

        // Xóa 1 sản phẩm khỏi giỏ
        public IActionResult RemoveFromCart(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _cart.RemoveFromCart(product);
            }
            return RedirectToAction("Index");
        }

        // Xóa toàn bộ giỏ hàng
        public IActionResult ClearCart()
        {
            _cart.ClearCart();
            return RedirectToAction("Index");
        }
    }
}