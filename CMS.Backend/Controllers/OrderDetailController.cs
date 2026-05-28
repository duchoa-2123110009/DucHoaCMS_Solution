/*
 * Ho va ten: LUU DUC HOA
 * Mssv: 2123110009
 * Ngay tao: 22/05/2026
 */
using CMS.Data;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Backend.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // "Tiêm" kết nối vào Controller
        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy dữ liệu THẬT từ bảng OrderDetails trong SQL
            var data = _context.OrderDetails.ToList();
            return View(data);
        }
    }
}
