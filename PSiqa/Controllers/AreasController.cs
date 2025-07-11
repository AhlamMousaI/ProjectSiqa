using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PSiqa.Data;
using PSiqa.Models;

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
            return View(await _context.Areas.ToListAsync());
        }

        // GET: Areas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        // GET: Areas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Area area)
        {
            if (_context.Areas.Any(a => a.Name == area.Name))
            {
                ModelState.AddModelError("Name", "اسم المنطقة موجود مسبقًا.");
                return View(area);
            }

            if (!ModelState.IsValid)
            {
                return View(area);
            }

            _context.Add(area);
            await _context.SaveChangesAsync();
            TempData["Success"] = "تمت إضافة المنطقة بنجاح!";
            return RedirectToAction(nameof(Index));
        }


        // GET: Areas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Area area)
        {
            if (id != area.Id) return NotFound();

            if (_context.Areas.Any(a => a.Name == area.Name && a.Id != area.Id))
            {
                ModelState.AddModelError("Name", "اسم المنطقة مستخدم من قبل.");
                return View(area);
            }

            if (!ModelState.IsValid)
            {
                return View(area);
            }

            try
            {
                _context.Update(area);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تعديل المنطقة بنجاح!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Areas.Any(a => a.Id == area.Id))
                    return NotFound();
                throw;
            }
        }



        // GET: Areas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _context.Areas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (area == null)
            {
                return NotFound();
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
            return _context.Areas.Any(e => e.Id == id);
        }
    }
}
