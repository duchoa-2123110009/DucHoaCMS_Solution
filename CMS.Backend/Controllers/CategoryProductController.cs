/*
 * Ho va ten: LUU DUC HOA
 * Mssv: 2123110009
 * Ngay tao: 22/05/2026
 */
using CMS.Data;
using CMS.Data.Entities; // Thêm để nhận diện model CategoryProduct
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class CategoryProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối vào Controller
        public CategoryProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Hiển thị danh sách danh mục sản phẩm
        public IActionResult Index()
        {
            // Lấy dữ liệu THẬT từ bảng CategoriesProducts trong SQL
            var data = _context.CategoriesProducts.ToList();
            return View(data);
        }

        // 2. GET: Xem chi tiết một danh mục sản phẩm
        public IActionResult Details(int id)
        {
            var category = _context.CategoriesProducts.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound(); // Trả về trang lỗi 404 nếu không tìm thấy
            }
            return View(category);
        }

        // 3. GET: Hiển thị form Thêm mới danh mục
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 4. POST: Thực hiện lưu danh mục mới vào CSDL
        [HttpPost]
        public IActionResult Create(CategoryProduct model)
        {
            _context.CategoriesProducts.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 5. GET: Hiển thị form Sửa kèm dữ liệu cũ của danh mục
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.CategoriesProducts.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // 6. POST: Thực hiện cập nhật thay đổi danh mục
        [HttpPost]
        public IActionResult Edit(CategoryProduct model)
        {
            _context.CategoriesProducts.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 7. GET/POST: Xóa danh mục sản phẩm theo Id
        public IActionResult Delete(int id)
        {
            var category = _context.CategoriesProducts.Find(id);
            if (category != null)
            {
                _context.CategoriesProducts.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}