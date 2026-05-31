using Microsoft.EntityFrameworkCore;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models.Interfaces;
namespace ShopVanPhongPham.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CartId;

        public ShoppingCartRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            CartId = GetCartId();
        }

        private string GetCartId()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            string? cartId = session.GetString("CartId");
            if (cartId == null)
            {
                cartId = Guid.NewGuid().ToString();
                session.SetString("CartId", cartId);
            }
            return cartId;
        }

        public void AddToCart(int productId)
        {
            var item = _context.ShoppingCartItems
                .FirstOrDefault(x => x.CartId == CartId && x.ProductId == productId);

            if (item == null)
            {
                _context.ShoppingCartItems.Add(new ShoppingCartItem
                {
                    CartId = CartId,
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            _context.SaveChanges();
            UpdateCartCount();
        }

        public void RemoveFromCart(int id)
        {
            var item = _context.ShoppingCartItems.Find(id);
            if (item != null)
            {
                _context.ShoppingCartItems.Remove(item);
                _context.SaveChanges();
            }
            UpdateCartCount();
        }

        public List<ShoppingCartItem> GetCartItems()
        {
            return _context.ShoppingCartItems
                .Include(x => x.Product)
                .Where(x => x.CartId == CartId)
                .ToList();
        }

        public int GetCartCount()
        {
            return _context.ShoppingCartItems
                .Where(x => x.CartId == CartId)
                .Sum(x => x.Quantity);
        }

        public void ClearCart()
        {
            var items = _context.ShoppingCartItems
                .Where(x => x.CartId == CartId).ToList();
            _context.ShoppingCartItems.RemoveRange(items);
            _context.SaveChanges();
            UpdateCartCount();
        }

        private void UpdateCartCount()
        {
            var count = GetCartCount();
            _httpContextAccessor.HttpContext!.Session.SetInt32("CartCount", count);
        }
    }
}
