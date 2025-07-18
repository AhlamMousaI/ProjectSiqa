using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSiqa.Data;
using PSiqa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PSiqa.Controllers
{
    public class AreasController : Controller
    {
        private readonly SSDbContext _context;

        public AreasController(SSDbContext context)
        {
            _context = context;
        }

        // GET: Areas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Areas.OrderBy(a => a.Name).ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var area = await _context.Areas
                .Include(a => a.Customers)
                .Include(a => a.TankAreas)
                .ThenInclude(ta => ta.Tank)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null) return NotFound();

            return View(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            ViewBag.Tanks = _context.Tanks.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.TankName
            }).ToList();

            return View();
        }

        // POST: Areas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Area area, int[] selectedTanks)
        {
            if (_context.Areas.Any(a => a.Name == area.Name))
            {
                ModelState.AddModelError("Name", "اسم المنطقة موجود مسبقًا");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(area);
                    await _context.SaveChangesAsync();

                    if (selectedTanks != null)
                    {
                        foreach (var tankId in selectedTanks)
                        {
                            _context.TankAreas.Add(new TankArea
                            {
                                AreaId = area.Id,
                                TankId = tankId
                            });
                        }
                        await _context.SaveChangesAsync();
                    }

                    TempData["ToastMessage"] = "تمت إضافة المنطقة بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء الحفظ: " + ex.Message);
                }
            }

            ViewBag.Tanks = _context.Tanks.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.TankName
            }).ToList();

            return View(area);
        }

        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var area = await _context.Areas
                .Include(a => a.TankAreas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null) return NotFound();

            ViewBag.Tanks = _context.Tanks.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.TankName,
                Selected = area.TankAreas.Any(ta => ta.TankId == t.Id)
            }).ToList();

            return View(area);
        }

        // POST: Areas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Area area, int[] selectedTanks)
        {
            if (id != area.Id) return NotFound();

            if (_context.Areas.Any(a => a.Name == area.Name && a.Id != area.Id))
            {
                ModelState.AddModelError("Name", "اسم المنطقة موجود مسبقًا");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // تحديث المنطقة
                    _context.Update(area);

                    // تحديث الخزانات المرتبطة
                    var existingTanks = await _context.TankAreas
                        .Where(ta => ta.AreaId == area.Id)
                        .ToListAsync();

                    _context.TankAreas.RemoveRange(existingTanks);

                    if (selectedTanks != null)
                    {
                        foreach (var tankId in selectedTanks)
                        {
                            _context.TankAreas.Add(new TankArea
                            {
                                AreaId = area.Id,
                                TankId = tankId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["ToastMessage"] = "تم تعديل المنطقة بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.Id))
                        return NotFound();
                    throw;
                }
            }

            ViewBag.Tanks = _context.Tanks.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.TankName,
                Selected = selectedTanks != null && selectedTanks.Contains(t.Id)
            }).ToList();

            return View(area);
        }

        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var area = await _context.Areas
                .Include(a => a.Customers)
                .Include(a => a.TankAreas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null) return NotFound();

            if (area.Customers.Any() || area.TankAreas.Any())
            {
                TempData["ToastError"] = "لا يمكن حذف المنطقة لأنها مرتبطة ببيانات أخرى";
                return RedirectToAction(nameof(Index));
            }

            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "تم حذف المنطقة بنجاح";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.Id == id);
        }
    }
}