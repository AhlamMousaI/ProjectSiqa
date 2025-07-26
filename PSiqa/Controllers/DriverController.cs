using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSiqa.Data;
using PSiqa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PSiqa.Controllers
{
    public class DriverController : Controller
    {
        private readonly SSDbContext _context;

        public DriverController(SSDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() =>
            View(await _context.Drivers.ToListAsync());

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Phone")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Drivers.Add(driver); 
                await _context.SaveChangesAsync();

               
                return RedirectToAction(nameof(Index));
            }

            return View(driver);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();
            return View(driver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Phone")] Driver driver)
        {
            if (id != driver.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) return NotFound();
            return View(driver);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);
            if (driver == null) return NotFound();
            return View(driver);
        }
    }
}
