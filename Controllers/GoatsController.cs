using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;

namespace ZambaFarm.Controllers
{
    public class GoatsController : Controller
    {
        private readonly FarmContext _context;

        public GoatsController(FarmContext context)
        {
            _context = context;
        }

        // GET: Goats
        public async Task<IActionResult> Index()
        {
            var farmContext = _context.Goats.Include(g => g.Mother);
            return View(await farmContext.ToListAsync());
        }

        // GET: Goats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Goats == null)
            {
                return NotFound();
            }

            var goat = await _context.Goats
                .Include(g => g.Mother)
                .FirstOrDefaultAsync(m => m.GoatId == id);
            if (goat == null)
            {
                return NotFound();
            }

            return View(goat);
        }
        [Authorize]
        // GET: Goats/Create
        public IActionResult Create()
        {
            ViewData["MotherId"] = new SelectList(_context.Goats, "GoatId", "GoatId");
            return View();
        }

        // POST: Goats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoatId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Goat goat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MotherId"] = new SelectList(_context.Goats, "GoatId", "GoatId", goat.MotherId);
            return View(goat);
        }

        // GET: Goats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Goats == null)
            {
                return NotFound();
            }

            var goat = await _context.Goats.FindAsync(id);
            if (goat == null)
            {
                return NotFound();
            }
            ViewData["MotherId"] = new SelectList(_context.Goats, "GoatId", "GoatId", goat.MotherId);
            return View(goat);
        }

        // POST: Goats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoatId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Goat goat)
        {
            if (id != goat.GoatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoatExists(goat.GoatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MotherId"] = new SelectList(_context.Goats, "GoatId", "GoatId", goat.MotherId);
            return View(goat);
        }

        // GET: Goats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Goats == null)
            {
                return NotFound();
            }

            var goat = await _context.Goats
                .Include(g => g.Mother)
                .FirstOrDefaultAsync(m => m.GoatId == id);
            if (goat == null)
            {
                return NotFound();
            }

            return View(goat);
        }

        // POST: Goats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Goats == null)
            {
                return Problem("Entity set 'FarmContext.Goats'  is null.");
            }
            var goat = await _context.Goats.FindAsync(id);
            if (goat != null)
            {
                _context.Goats.Remove(goat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoatExists(int id)
        {
          return (_context.Goats?.Any(e => e.GoatId == id)).GetValueOrDefault();
        }
    }
}
