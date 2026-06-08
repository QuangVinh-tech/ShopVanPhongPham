using Microsoft.EntityFrameworkCore;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Models.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IShoppingCartRepository _cart;

        public OrderRepository(AppDbContext context, IShoppingCartRepository cart)
        {
            _context = context;
            _cart = cart;
        }

        public Order PlaceOrder(Order order)
        {
            var cartItems = _cart.GetCartItems();
            order.OrderDetails = new List<OrderDetail>();

            foreach (var item in cartItems)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.Product!.Id,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
            }

            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _cart.GetCartTotal();

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }
        public List<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderPlaced)
                .ToList();
        }
    }
}