using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ZambaFarm.Controllers
{
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
            var farmContext = _context.Rabbits.ToListAsync(); // Just get the rabbits without the mother relationship
            return View(await farmContext);
        }

        // GET: Rabbits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rabbits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate")] Rabbit rabbit)
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
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", validationErrors);
            }
            return View(rabbit);
        }

        // GET: Rabbits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rabbit = await _context.Rabbits.FindAsync(id);
            if (rabbit == null)
            {
                return NotFound();
            }
            return View(rabbit);
        }

        // POST: Rabbits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate")] Rabbit rabbit)
        {
            if (id != rabbit.RabbitId)
            {
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
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(rabbit);
        }

        // GET: Rabbits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rabbit = await _context.Rabbits
                .FirstOrDefaultAsync(m => m.RabbitId == id);

            if (rabbit == null)
            {
                return NotFound();
            }

            return View(rabbit);
        }


        // GET: Rabbits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rabbit = await _context.Rabbits
                .FirstOrDefaultAsync(m => m.RabbitId == id);
            if (rabbit == null)
            {
                return NotFound();
            }

            return View(rabbit);
        }

        // POST: Rabbits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rabbit = await _context.Rabbits.FindAsync(id);
            _context.Rabbits.Remove(rabbit);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Rabbit deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool RabbitExists(int id)
        {
            return _context.Rabbits.Any(e => e.RabbitId == id);
        }
    }
}
