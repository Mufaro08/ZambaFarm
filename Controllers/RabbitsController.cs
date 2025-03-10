using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;

namespace ZambaFarm.Controllers
{
     // Enforce authentication for all actions
    public class RabbitsController : Controller
    {
        private readonly FarmContext _context;

        public RabbitsController(FarmContext context)
        {
            _context = context;
        }

        // GET: Rabbits
        public async Task<IActionResult> Index()
        {
            var farmContext = _context.Rabbits.Include(r => r.Mother);
            return View(await farmContext.ToListAsync());
        }

        // GET: Rabbits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rabbits == null)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            var rabbit = await _context.Rabbits
                .Include(r => r.Mother)
                .FirstOrDefaultAsync(m => m.RabbitId == id);

            if (rabbit == null)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            return View(rabbit);
        }

        // GET: Rabbits/Create
        public IActionResult Create()
        {
            ViewData["MotherId"] = new SelectList(_context.Rabbits, "RabbitId", "RabbitId");
            return View();
        }

        // POST: Rabbits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Rabbit rabbit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(rabbit);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rabbit added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error adding rabbit: {ex.Message}";
                }
            }
            else
            {
                // Capture validation errors
                var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                        .Select(e => e.ErrorMessage)
                                                        .ToList();
                TempData["ErrorMessage"] = "Validation failed for the following reasons: " + string.Join(", ", validationErrors);
            }

            ViewData["MotherId"] = new SelectList(_context.Rabbits, "RabbitId", "RabbitId", rabbit.MotherId);
            return View(rabbit);
        }


        // GET: Rabbits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rabbits == null)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            var rabbit = await _context.Rabbits.FindAsync(id);
            if (rabbit == null)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            ViewData["MotherId"] = new SelectList(_context.Rabbits, "RabbitId", "RabbitId", rabbit.MotherId);
            return View(rabbit);
        }

        // POST: Rabbits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,MotherId")] Rabbit rabbit)
        {
            if (id != rabbit.RabbitId)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rabbit);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rabbit updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RabbitExists(rabbit.RabbitId))
                    {
                        TempData["ErrorMessage"] = "Rabbit not found.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error updating rabbit: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation failed. Please check your input.";
            }

            ViewData["MotherId"] = new SelectList(_context.Rabbits, "RabbitId", "RabbitId", rabbit.MotherId);
            return View(rabbit);
        }

        // GET: Rabbits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rabbits == null)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            var rabbit = await _context.Rabbits
                .Include(r => r.Mother)
                .FirstOrDefaultAsync(m => m.RabbitId == id);

            if (rabbit == null)
            {
                TempData["ErrorMessage"] = "Rabbit not found.";
                return NotFound();
            }

            return View(rabbit);
        }
        [Authorize]
        // POST: Rabbits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rabbits == null)
            {
                TempData["ErrorMessage"] = "Entity set 'FarmContext.Rabbits' is null.";
                return Problem("Entity set 'FarmContext.Rabbits' is null.");
            }

            try
            {
                var rabbit = await _context.Rabbits.FindAsync(id);
                if (rabbit != null)
                {
                    _context.Rabbits.Remove(rabbit);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rabbit deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Rabbit not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting rabbit: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RabbitExists(int id)
        {
            return (_context.Rabbits?.Any(e => e.RabbitId == id)).GetValueOrDefault();
        }
    }
}
