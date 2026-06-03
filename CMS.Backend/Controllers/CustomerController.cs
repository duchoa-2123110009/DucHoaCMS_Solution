/*
 * Ho va ten: LUU DUC HOA
 * Mssv: 2123110009
 * Ngay tao: 22/05/2026
 */
using CMS.Data;
using CMS.Data.Entities; // Thêm để nhận diện model Customer
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using System.Linq;


namespace CMS.Backend.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối vào Controller
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Hiển thị danh sách khách hàng
        public IActionResult Index()
        {
            // Lấy dữ liệu THẬT từ bảng Customers trong SQL
            var data = _context.Customers.ToList();
            return View(data);
        }

        // 2. GET: Xem chi tiết thông tin một khách hàng
        public IActionResult Details(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound(); // Trả về trang lỗi 404 nếu không tìm thấy
            }
            return View(customer);
        }

        // 3. GET: Hiển thị form Thêm mới khách hàng
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 4. POST: Thực hiện lưu khách hàng mới vào CSDL
        [HttpPost]
        public IActionResult Create(Customer model)
        {
            // Thêm vào bộ nhớ tạm
            _context.Customers.Add(model);

            // Cập nhật xuống SQL Server
            _context.SaveChanges();

            // Quay về trang danh sách
            return RedirectToAction("Index");
        }

        // 5. GET: Hiển thị form Sửa kèm dữ liệu cũ của khách hàng
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // 6. POST: Thực hiện cập nhật thay đổi của khách hàng
        [HttpPost]
        public IActionResult Edit(Customer model)
        {
            // Cập nhật trạng thái thay đổi của model
            _context.Customers.Update(model);

            // Lưu thay đổi vào CSDL
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 7. GET/POST: Xóa khách hàng theo Id
        public IActionResult Delete(int id)
        {
            // Tìm khách hàng theo Id
            var customer = _context.Customers.Find(id);

            if (customer != null)
            {
                // Xóa khỏi bộ nhớ tạm
                _context.Customers.Remove(customer);

                // Cập nhật xuống SQL Server
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}