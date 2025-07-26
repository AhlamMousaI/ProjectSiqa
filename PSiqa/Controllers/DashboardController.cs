using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSiqa.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PSiqa.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SSDbContext _context;

        public DashboardController(SSDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var completedOrders = await _context.Orders
                .Where(o => o.Status == "تم")
                .CountAsync();

            var mostRequestedArea = await _context.Customers
                .GroupBy(c => c.Area)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            var totalTankCapacity = await _context.Tanks
                .SumAsync(t => t.Capacity);

            ViewBag.CompletedOrders = completedOrders;
            ViewBag.MostRequestedArea = mostRequestedArea?.ToString() ?? "No data available";
            ViewBag.TotalTankCapacity = totalTankCapacity;

            return View();
        }




    }
}

