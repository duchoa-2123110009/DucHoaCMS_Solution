using CMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var details = _context.OrderDetails
                .Select(x => new
                {
                    x.Id,
                    x.OrderId,
                    x.ProductId,
                    x.Quantity,
                    x.UnitPrice
                })
                .ToList();

            return Ok(details);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var detail = _context.OrderDetails
                .FirstOrDefault(x => x.Id == id);

            if (detail == null)
                return NotFound(new { message = "Không tìm thấy chi tiết đơn hàng" });

            return Ok(detail);
        }
    }
}