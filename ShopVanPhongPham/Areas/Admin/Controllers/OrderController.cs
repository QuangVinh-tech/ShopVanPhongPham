using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Data;
using ShopVanPhongPham.Models;
using ShopVanPhongPham.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShopVanPhongPham.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly AppDbContext _context;

        public OrderController(IOrderRepository orderRepo, AppDbContext context)
        {
            _orderRepo = orderRepo;
            _context = context;
        }

        // GET /Admin/Order
        public IActionResult Index(string? status)
        {
            var orders = _orderRepo.GetAllOrders();

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.Status == status).ToList();

            ViewBag.StatusFilter = status;
            ViewBag.StatusList = new List<string>
            {
                "Chờ xử lý", "Đang giao", "Đã giao", "Đã hủy"
            };

            // Thống kê nhanh
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.PendingOrders = _context.Orders.Count(o => o.Status == null || o.Status == "Chờ xử lý");
            ViewBag.DoneOrders = _context.Orders.Count(o => o.Status == "Đã giao");

            return View(orders);
        }

        // POST /Admin/Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int orderId, string status)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null) return NotFound();

            order.Status = status;
            _context.SaveChanges();

            TempData["Success"] = $"Đã cập nhật đơn #{orderId} → {status}";
            return RedirectToAction("Index");
        }

        // GET /Admin/Order/Detail/5
        public IActionResult Detail(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }
    }
}
