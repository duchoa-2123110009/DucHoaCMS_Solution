/*
 * Ho va ten: LUU DUC HOA
 * Mssv: 2123110009
 * Ngay tao: 22/05/2026
 */
using CMS.Data;
using CMS.Data.Entities; // Thêm để nhận diện các Model
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Hỗ trợ Eager Loading liên kết nhiều bảng
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối vào Controller
        public OrderDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Hiển thị danh sách chi tiết đơn hàng
        public IActionResult Index()
        {
            // Lấy dữ liệu THẬT và kết nối với bảng Orders, Products để lấy thông tin chi tiết
            var data = _context.OrderDetails
                               .Include(od => od.Order)
                               .Include(od => od.Product)
                               .ToList();
            return View(data);
        }

        // 2. GET: Xem chi tiết một dòng hóa đơn
        public IActionResult Details(int id)
        {
            var orderDetail = _context.OrderDetails
                                      .Include(od => od.Order)
                                      .Include(od => od.Product)
                                      .FirstOrDefault(od => od.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        // 3. GET: Hiển thị form thêm sản phẩm vào đơn hàng
        [HttpGet]
        public IActionResult Create()
        {
            // Chuẩn bị danh sách Đơn hàng và Sản phẩm đổ vào ViewBag dropdown
            ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id");
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // 4. POST: Thực hiện lưu chi tiết đơn hàng mới
        [HttpPost]
        public IActionResult Create(OrderDetail model)
        {
            _context.OrderDetails.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 5. GET: Hiển thị form Sửa dữ liệu cũ
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail == null) return NotFound();

            ViewBag.OrderList = new SelectList(_context.Orders, "Id", "Id", orderDetail.OrderId);
            ViewBag.ProductList = new SelectList(_context.Products, "Id", "Name", orderDetail.ProductId);
            return View(orderDetail);
        }

        // 6. POST: Thực hiện cập nhật thay đổi
        [HttpPost]
        public IActionResult Edit(OrderDetail model)
        {
            _context.OrderDetails.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 7. GET/POST: Xóa một mục chi tiết đơn hàng
        public IActionResult Delete(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}