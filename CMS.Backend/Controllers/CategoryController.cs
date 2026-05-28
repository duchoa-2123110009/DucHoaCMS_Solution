// Họ tên:Lưu Đức Hòa | MSV: 2123110009
// Buổi 2: Thay mock data (Buổi 1) bằng dữ liệu thật từ bảng Categories
//Version: 1.2
using CMS.Data;
using CMS.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS.Backend.Controllers;

/// <summary>
/// Controller quản lý danh mục tin tức (bảng Categories).
/// Buổi 1: dùng List&lt;Category&gt; giả trong code.
/// Buổi 2: gọi _context.Categories từ SQL Server.
/// </summary>
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /Category/Index — hiển thị bảng danh mục.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        // Lấy toàn bộ dòng trong bảng Categories
        var data = await _context.Categories.ToListAsync();

        // Gửi sang Views/Category/Index.cshtml (IEnumerable&lt;Category&gt;)
        return View(data);
    }
    // 1. Hàm GET: Dùng để hiển thị giao diện Form cho  nhập
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // 2. Hàm POST: Dùng để đón dữ liệu từ Form gửi lên và lưu vào SQL
    [HttpPost]
    public IActionResult Create(Category model)
    {
        // BƯỚC 1: Thêm dữ liệu vào bộ nhớ tạm của Entity Framework
        _context.Categories.Add(model);

        // BƯỚC 2: Ra lệnh cho hệ thống ghi dữ liệu thật sự vào SQL Server
        _context.SaveChanges();

        // Sau khi lưu thành công, tự động quay về trang danh sách
        return RedirectToAction("Index");
    }
    // Action nhận vào Id của danh mục cần xóa
    public IActionResult Delete(int id)
    {
        // Bước 1: Tìm đối tượng danh mục trong Database bằng Id
        var category = _context.Categories.Find(id);

        // Kiểm tra nếu tìm thấy thì mới xóa
        if (category != null)
        {
            // Bước 2: Lệnh xóa khỏi bộ nhớ tạm (Tracking)
            _context.Categories.Remove(category);

            // Bước 3: Chốt phiên làm việc, xóa thực sự trong SQL Server
            _context.SaveChanges();
        }

        // Sau khi xóa xong, quay lại trang danh sách để cập nhật giao diện
        return RedirectToAction("Index");
    }
    // 1. Hàm GET: Tìm dữ liệu cũ và đổ lên Form
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Tìm danh mục trong Database theo Id [cite: 348, 350]
        var category = _context.Categories.Find(id);

        if (category == null) return NotFound();

        return View(category); // Gửi đối tượng tìm được sang giao diện Edit
    }

    // 2. Hàm POST: Nhận dữ liệu mới từ người dùng và lưu lại
    [HttpPost]
    public IActionResult Edit(Category model)
    {
        // Lệnh cập nhật đối tượng vào bộ nhớ tạm
        _context.Categories.Update(model);

        // Lưu thay đổi thực sự xuống SQL Server [cite: 504, 509]
        _context.SaveChanges();

        // Quay lại trang danh sách để xem kết quả
        return RedirectToAction("Index");
    }


}
