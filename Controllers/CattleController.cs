using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;

namespace ZambaFarm.Controllers
{
    public class CattleController : Controller
    {
        private readonly FarmContext _context;

        public CattleController(FarmContext context)
        {
            _context = context;
        }

        // GET: Cattle
        public async Task<IActionResult> Index()
        {
            var farmContext = _context.Cattles.Include(c => c.Mother);
            return View(await farmContext.ToListAsync());
        }

        // GET: Cattle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cattles == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattles
                .Include(c => c.Mother)
                .FirstOrDefaultAsync(m => m.CattleId == id);
            if (cattle == null)
            {
                return NotFound();
            }

            return View(cattle);
        }

        // GET: Cattle/Create
        public IActionResult Create()
        {
            ViewData["MotherId"] = new SelectList(_context.Cattles, "CattleId", "CattleId");
            return View();
        }

        // POST: Cattle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CattleId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Cattle cattle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cattle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MotherId"] = new SelectList(_context.Cattles, "CattleId", "CattleId", cattle.MotherId);
            return View(cattle);
        }

        // GET: Cattle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cattles == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattles.FindAsync(id);
            if (cattle == null)
            {
                return NotFound();
            }
            ViewData["MotherId"] = new SelectList(_context.Cattles, "CattleId", "CattleId", cattle.MotherId);
            return View(cattle);
        }

        // POST: Cattle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CattleId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Cattle cattle)
        {
            if (id != cattle.CattleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cattle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CattleExists(cattle.CattleId))
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
            ViewData["MotherId"] = new SelectList(_context.Cattles, "CattleId", "CattleId", cattle.MotherId);
            return View(cattle);
        }

        // GET: Cattle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cattles == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattles
                .Include(c => c.Mother)
                .FirstOrDefaultAsync(m => m.CattleId == id);
            if (cattle == null)
            {
                return NotFound();
            }

            return View(cattle);
        }

        // POST: Cattle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cattles == null)
            {
                return Problem("Entity set 'FarmContext.Cattles'  is null.");
            }
            var cattle = await _context.Cattles.FindAsync(id);
            if (cattle != null)
            {
                _context.Cattles.Remove(cattle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CattleExists(int id)
        {
          return (_context.Cattles?.Any(e => e.CattleId == id)).GetValueOrDefault();
        }
    }
}
