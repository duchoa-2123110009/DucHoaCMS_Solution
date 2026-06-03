using CMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.CategoriesProducts
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description
                })
                .ToList();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var category = _context.CategoriesProducts
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Không tìm thấy danh mục" });

            return Ok(category);
        }
    }
}