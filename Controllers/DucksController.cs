using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ZambaFarm.Controllers
{
    public class DucksController : Controller
    {
        private readonly FarmContext _context;

        public DucksController(FarmContext context)
        {
            _context = context;
        }

        // GET: Ducks
        public async Task<IActionResult> Index()
        {
            var farmContext = await _context.Ducks.ToListAsync();
            return View(farmContext);
        }

        // GET: Ducks/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Duck());
        }

        // POST: Ducks/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DuckId,TagNumber,Gender,BirthDate,IsEggLaying,NumberOfEggs,IsMating,MatingDate,MotherDuckId,MotherTagNumber")] Duck duck)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(duck);
            }

            try
            {
                // Add the main duck (mother)
                _context.Ducks.Add(duck);
                await _context.SaveChangesAsync(); // Save Duck first to get ID

                // Generate eggs if the Duck is laying eggs
                if (duck.IsEggLaying && duck.NumberOfEggs.HasValue)
                {
                    for (int i = 0; i < duck.NumberOfEggs.Value; i++)
                    {
                        var egg = new Duck
                        {
                            TagNumber = $"Egg-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                            Gender = "Unknown",
                            BirthDate = DateTime.Now,
                            MotherTagNumber = duck.TagNumber,
                            MotherDuckId = duck.DuckId
                        };
                        _context.Ducks.Add(egg);
                    }
                    await _context.SaveChangesAsync(); // Save all eggs
                }

                TempData["SuccessMessage"] = "Duck added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding duck: {ex.Message}";
                return View(duck);
            }
        }

        // GET: TDucks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Duck ID not specified." });
            }

            try
            {
                var duck = await _context.Ducks
                    .Include(t => t.Offspring)
                    .FirstOrDefaultAsync(t => t.DuckId == id);

                if (duck == null)
                {
                    return RedirectToAction("Error", new { message = "Duck not found." });
                }

                return View(duck);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading Duck details: {ex.Message}" });
            }
        }

        // GET: Ducks/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Duck ID not specified." });
            }

            try
            {
                var duck = await _context.Ducks.FindAsync(id);
                if (duck == null)
                {
                    return RedirectToAction("Error", new { message = "Duck not found." });
                }
                return View(duck);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading duck for edit: {ex.Message}" });
            }
        }

        // POST: Ducks/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DuckId,TagNumber,Gender,BirthDate,IsEggLaying,NumberOfEggs,IsMating,MatingDate,MotherDuckId,MotherTagNumber, Offspring")] Duck duck)
        {
            if (id != duck.DuckId)
            {
                return RedirectToAction("Error", new { message = "Duck ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(duck);
            }

            try
            {
                // Update the main duck
                _context.Update(duck);
                await _context.SaveChangesAsync(); // Save the updated duck

                // Update offspring (if any)
                if (duck.Offspring.Any())
                {
                    foreach (var egg in duck.Offspring)
                    {
                        egg.MotherDuckId = duck.DuckId; // Ensure the offspring is linked to the mother
                        _context.Update(egg); // Update each egg
                    }
                    await _context.SaveChangesAsync(); // Save all changes to the offspring
                }

                TempData["SuccessMessage"] = "Duck updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating duck: {ex.Message}";
                return View(duck);
            }
        }

        // GET: Ducks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Duck ID not specified." });
            }

            try
            {
                var duck = await _context.Ducks.FirstOrDefaultAsync(m => m.DuckId == id);
                if (duck == null)
                {
                    return RedirectToAction("Error", new { message = "Duck not found." });
                }

                return View(duck);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading duck for delete: {ex.Message}" });
            }
        }

        // POST: Ducks/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var duck = await _context.Ducks.FindAsync(id);
                if (duck == null)
                {
                    return RedirectToAction("Error", new { message = "Duck not found." });
                }

                _context.Ducks.Remove(duck);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Duck deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting duck: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DuckExists(int id)
        {
            return _context.Ducks.Any(e => e.DuckId == id);
        }

        // Custom Error Action to Handle Errors and Pass ErrorViewModel
        public IActionResult Error(string message)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            };

            return View("Error", model);
        }
    }
}
