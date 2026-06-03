/*
 * Ho va ten: LUU DUC HOA
 * Mssv: 2123110009
 * Ngay tao: 22/05/2026
 */
using CMS.Data;
using CMS.Data.Entities; // Thêm để nhận diện Product và CategoryProduct
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; 
// Hỗ trợ .Include()
using System.IO;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối vào Controller
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Hiển thị danh sách sản phẩm kèm tên danh mục
        public IActionResult Index()
        {
            var data = _context.Products
                               .Include(p => p.CategoryProduct) // Kết nối sang bảng danh mục
                               .ToList();
            return View(data);
        }

        // 2. GET: Xem chi tiết một sản phẩm
        public IActionResult Details(int id)
        {
            var product = _context.Products
                                  .Include(p => p.CategoryProduct)
                                  .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // 3. GET: Hiển thị giao diện Thêm mới sản phẩm
        [HttpGet]
        public IActionResult Create()
        {
            // Đổ danh sách danh mục sản phẩm vào Dropdownlist
            ViewBag.CategoryList = new SelectList(_context.CategoriesProducts, "Id", "Name");
            return View();
        }

        // 4. POST: Thực hiện lưu sản phẩm mới kèm xử lý hình ảnh
        [HttpPost]
        public IActionResult Create(Product model, IFormFile uploadImage)
        {
            if (uploadImage != null && uploadImage.Length > 0)
            {
                // Đặt tên file duy nhất và lưu vào thư mục wwwroot/images
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }
                model.ImageUrl = "/images/" + fileName; // Lưu đường dẫn vào Database
            }

            _context.Products.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 5. GET: Giao diện sửa thông tin sản phẩm
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.CategoryList = new SelectList(_context.CategoriesProducts, "Id", "Name", product.CategoryProductId);
            return View(product);
        }

        // 6. POST: Thực hiện cập nhật thông tin sản phẩm
        [HttpPost]
        public IActionResult Edit(Product model, IFormFile uploadImage)
        {
            if (uploadImage != null && uploadImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadImage.CopyTo(stream);
                }
                model.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 7. GET/POST: Xóa sản phẩm khỏi hệ thống
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}