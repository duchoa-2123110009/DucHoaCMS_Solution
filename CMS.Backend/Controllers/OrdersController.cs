using CMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Select(x => new
                {
                    x.Id,
                    x.OrderDate,
                    x.Status,
                    x.Notes,
                    CustomerName = x.Customer.FullName
                })
                .ToList();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var order = _context.Orders
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });

            return Ok(order);
        }
    }
}