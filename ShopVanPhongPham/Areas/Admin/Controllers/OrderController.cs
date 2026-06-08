using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopVanPhongPham.Models.Interfaces;

namespace ShopVanPhongPham.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;

        public OrderController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public IActionResult Index()
        {
            var orders = _orderRepo.GetAllOrders();
            return View(orders);
        }
    }
}