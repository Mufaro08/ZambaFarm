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
    public class PigsController : Controller
    {
        private readonly FarmContext _context;

        public PigsController(FarmContext context)
        {
            _context = context;
        }

        // GET: Pigs
        public async Task<IActionResult> Index()
        {
            var farmContext = _context.Pigs.Include(p => p.Mother);
            return View(await farmContext.ToListAsync());
        }

        // GET: Pigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pigs == null)
            {
                return NotFound();
            }

            var pig = await _context.Pigs
                .Include(p => p.Mother)
                .FirstOrDefaultAsync(m => m.PigId == id);
            if (pig == null)
            {
                return NotFound();
            }

            return View(pig);
        }

        // GET: Pigs/Create
        public IActionResult Create()
        {
            ViewData["MotherId"] = new SelectList(_context.Pigs, "PigId", "PigId");
            return View();
        }

        // POST: Pigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PigId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Pig pig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MotherId"] = new SelectList(_context.Pigs, "PigId", "PigId", pig.MotherId);
            return View(pig);
        }

        // GET: Pigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pigs == null)
            {
                return NotFound();
            }

            var pig = await _context.Pigs.FindAsync(id);
            if (pig == null)
            {
                return NotFound();
            }
            ViewData["MotherId"] = new SelectList(_context.Pigs, "PigId", "PigId", pig.MotherId);
            return View(pig);
        }

        // POST: Pigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PigId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Pig pig)
        {
            if (id != pig.PigId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PigExists(pig.PigId))
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
            ViewData["MotherId"] = new SelectList(_context.Pigs, "PigId", "PigId", pig.MotherId);
            return View(pig);
        }

        // GET: Pigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pigs == null)
            {
                return NotFound();
            }

            var pig = await _context.Pigs
                .Include(p => p.Mother)
                .FirstOrDefaultAsync(m => m.PigId == id);
            if (pig == null)
            {
                return NotFound();
            }

            return View(pig);
        }

        // POST: Pigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pigs == null)
            {
                return Problem("Entity set 'FarmContext.Pigs'  is null.");
            }
            var pig = await _context.Pigs.FindAsync(id);
            if (pig != null)
            {
                _context.Pigs.Remove(pig);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PigExists(int id)
        {
          return (_context.Pigs?.Any(e => e.PigId == id)).GetValueOrDefault();
        }
    }
}
