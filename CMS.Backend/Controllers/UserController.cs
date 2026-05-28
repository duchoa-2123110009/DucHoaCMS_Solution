// Họ tên:Lưu Đức Hòa | MSV: 2123110009
// Buổi 2: Controller quản lý thành viên — lấy dữ liệu từ SQL Server qua EF Core
//Version: 1.2
using CMS.Data;                           // ApplicationDbContext (lớp kết nối database)
using CMS.Data.Entities;                  // ĐÃ THÊM: Để sửa lỗi không tìm thấy kiểu 'User'
using Microsoft.AspNetCore.Mvc;          // Controller, IActionResult, View()
using Microsoft.EntityFrameworkCore;     // ToListAsync() — truy vấn bất đồng bộ

namespace CMS.Backend.Controllers;

/// <summary>
/// Controller xử lý trang quản lý người dùng (bảng Users).
/// URL mặc định: /User hoặc /User/Index
/// </summary>
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Action Index: hiển thị danh sách thành viên.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.ToListAsync();
        return View(users);
    }

    // GET: Hiển thị form kèm dữ liệu cũ của User
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        return View(user);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(User model)
    {
        // Kiểm tra xem tên đăng nhập đã tồn tại chưa
        var checkExist = _context.Users.Any(u => u.Username == model.Username);
        if (checkExist)
        {
            ModelState.AddModelError("Username", "Tên đăng nhập này đã có người dùng!");
            return View(model);
        }

        // Lưu User mới vào Database
        _context.Users.Add(model);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // POST: Thực hiện lưu thay đổi
    [HttpPost]
    public IActionResult Edit(User model, string NewPassword)
    {
        var existingUser = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == model.Id);

        if (existingUser == null) return NotFound();

        if (!string.IsNullOrEmpty(NewPassword))
        {
            model.PasswordHash = NewPassword;
        }
        else
        {
            model.PasswordHash = existingUser.PasswordHash;
        }

        _context.Users.Update(model);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult Delete(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

}