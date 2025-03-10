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
    public class TurkeysController : Controller
    {
        private readonly FarmContext _context;

        public TurkeysController(FarmContext context)
        {
            _context = context;
        }

        // GET: Turkeys
        public async Task<IActionResult> Index()
        {
            var farmContext = _context.Turkeys.Include(t => t.Mother);
            return View(await farmContext.ToListAsync());
        }

        // GET: Turkeys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Turkeys == null)
            {
                return NotFound();
            }

            var turkey = await _context.Turkeys
                .Include(t => t.Mother)
                .FirstOrDefaultAsync(m => m.TurkeyId == id);
            if (turkey == null)
            {
                return NotFound();
            }

            return View(turkey);
        }

        // GET: Turkeys/Create
        public IActionResult Create()
        {
            ViewData["MotherId"] = new SelectList(_context.Turkeys, "TurkeyId", "TurkeyId");
            return View();
        }

        // POST: Turkeys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TurkeyId,TagNumber,Gender,BirthDate,IsBreeding,BreedingDate,Weight,MotherId")] Turkey turkey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turkey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MotherId"] = new SelectList(_context.Turkeys, "TurkeyId", "TurkeyId", turkey.MotherId);
            return View(turkey);
        }

        // GET: Turkeys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Turkeys == null)
            {
                return NotFound();
            }

            var turkey = await _context.Turkeys.FindAsync(id);
            if (turkey == null)
            {
                return NotFound();
            }
            ViewData["MotherId"] = new SelectList(_context.Turkeys, "TurkeyId", "TurkeyId", turkey.MotherId);
            return View(turkey);
        }

        // POST: Turkeys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TurkeyId,TagNumber,Gender,BirthDate,IsBreeding,BreedingDate,Weight,MotherId")] Turkey turkey)
        {
            if (id != turkey.TurkeyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turkey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurkeyExists(turkey.TurkeyId))
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
            ViewData["MotherId"] = new SelectList(_context.Turkeys, "TurkeyId", "TurkeyId", turkey.MotherId);
            return View(turkey);
        }

        // GET: Turkeys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Turkeys == null)
            {
                return NotFound();
            }

            var turkey = await _context.Turkeys
                .Include(t => t.Mother)
                .FirstOrDefaultAsync(m => m.TurkeyId == id);
            if (turkey == null)
            {
                return NotFound();
            }

            return View(turkey);
        }

        // POST: Turkeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Turkeys == null)
            {
                return Problem("Entity set 'FarmContext.Turkeys'  is null.");
            }
            var turkey = await _context.Turkeys.FindAsync(id);
            if (turkey != null)
            {
                _context.Turkeys.Remove(turkey);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurkeyExists(int id)
        {
          return (_context.Turkeys?.Any(e => e.TurkeyId == id)).GetValueOrDefault();
        }
    }
}
