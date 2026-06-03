using CMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _context.Customers
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.Phone,
                    x.Address
                })
                .ToList();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var customer = _context.Customers
                .FirstOrDefault(x => x.Id == id);

            if (customer == null)
                return NotFound(new { message = "Không tìm thấy khách hàng" });

            return Ok(customer);
        }
    }
}