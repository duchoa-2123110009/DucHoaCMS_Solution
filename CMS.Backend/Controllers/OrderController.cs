/*
 * Ho va ten: LUU DUC HOA
 * Mssv: 2123110009
 * Ngay tao: 22/05/2026
 */
using CMS.Data;
using CMS.Data.Entities; // Thêm để nhận diện model Order và Customer
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Hỗ trợ .Include() để lấy thông tin Khách hàng từ bảng khác
using System;
using System.Linq;

namespace CMS.Backend.Controllers
    {
        public class OrderController : Controller
        {
            private readonly ApplicationDbContext _context;

            // "Tiêm" kết nối vào Controller
            public OrderController(ApplicationDbContext context)
            {
                _context = context;
            }

            // 1. GET: Hiển thị danh sách đơn hàng (kèm tên khách hàng nhờ Eager Loading)
            public IActionResult Index()
            {
                // Lấy dữ liệu THẬT từ bảng Orders và liên kết với bảng Customers
                var data = _context.Orders
                                   .Include(o => o.Customer)
                                   .OrderByDescending(o => o.OrderDate)
                                   .ToList();
                return View(data);
            }

            // 2. GET: Xem chi tiết một đơn hàng
            public IActionResult Details(int id)
            {
                var order = _context.Orders
                                    .Include(o => o.Customer)
                                    .FirstOrDefault(o => o.Id == id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }

            // 3. GET: Hiển thị form Tạo đơn hàng mới
            [HttpGet]
            public IActionResult Create()
            {
                // Lấy danh sách Khách hàng để chọn khi lập đơn
                ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName");
                return View();
            }

            // 4. POST: Thực hiện lưu đơn hàng vào CSDL
            [HttpPost]
            public IActionResult Create(Order model)
            {
                if (model.OrderDate == DateTime.MinValue)
                {
                    model.OrderDate = DateTime.Now;
                }

                _context.Orders.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // 5. GET: Hiển thị form Sửa đơn hàng kèm dữ liệu cũ
            [HttpGet]
            public IActionResult Edit(int id)
            {
                var order = _context.Orders.Find(id);
                if (order == null) return NotFound();

                // Chuẩn bị danh sách khách hàng để chọn lại nếu cần
                ViewBag.CustomerList = new SelectList(_context.Customers, "Id", "FullName", order.CustomerId);
                return View(order);
            }

            // 6. POST: Thực hiện cập nhật thay đổi của đơn hàng
            [HttpPost]
            public IActionResult Edit(Order model)
            {
                _context.Orders.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // 7. GET/POST: Xóa đơn hàng theo Id
            public IActionResult Delete(int id)
            {
                var order = _context.Orders.Find(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }