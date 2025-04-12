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
    public class ChickensController : Controller
    {
        private readonly FarmContext _context;

        public ChickensController(FarmContext context)
        {
            _context = context;
        }

        // GET: Chickens
        public async Task<IActionResult> Index()
        {
            var farmContext = await _context.Chickens.ToListAsync();
            return View(farmContext);
        }

        // GET: Chickens/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Chicken());
        }

        // POST: Chickens/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChickenId,TagNumber,Gender,BirthDate,IsEggLaying,NumberOfEggs,IsMating,MatingDate,MotherChickenId,MotherTagNumber")] Chicken chicken)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(chicken);
            }

            try
            {
                // Add the main Chicken (mother)
                _context.Chickens.Add(chicken);
                await _context.SaveChangesAsync(); // Save Chicken first to get ID

                // Generate eggs if the Chicken is laying eggs
                if (chicken.IsEggLaying && chicken.NumberOfEggs.HasValue)
                {
                    for (int i = 0; i < chicken.NumberOfEggs.Value; i++)
                    {
                        var egg = new Chicken
                        {
                            TagNumber = $"Egg-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                            Gender = "Unknown",
                            BirthDate = DateTime.Now,
                            MotherTagNumber = chicken.TagNumber,
                            MotherChickenId = chicken.ChickenId
                        };
                        _context.Chickens.Add(egg);
                    }
                    await _context.SaveChangesAsync(); // Save all eggs
                }

                TempData["SuccessMessage"] = "Chicken added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding chicken: {ex.Message}";
                return View(chicken);
            }
        }

        // GET: Chickens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Chicken ID not specified." });
            }

            try
            {
                var chicken = await _context.Chickens
                    .Include(t => t.Offspring)
                    .FirstOrDefaultAsync(t => t.ChickenId == id);

                if (chicken == null)
                {
                    return RedirectToAction("Error", new { message = "Chicken not found." });
                }

                return View(chicken);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading chicken details: {ex.Message}" });
            }
        }

        // GET: Chickens/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Chicken ID not specified." });
            }

            try
            {
                var chicken = await _context.Chickens.FindAsync(id);
                if (chicken == null)
                {
                    return RedirectToAction("Error", new { message = "Chicken not found." });
                }
                return View(chicken);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading chicken for edit: {ex.Message}" });
            }
        }

            // POST: Chickens/Edit/5
            [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChickenId,TagNumber,Gender,BirthDate,IsEggLaying,NumberOfEggs,IsMating,MatingDate,MotherTurkeyId,MotherTagNumber, Offspring")] Chicken chicken)
        {
            if (id != chicken.ChickenId)
            {
                return RedirectToAction("Error", new { message = "Chicken ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(chicken);
            }

            try
            {
                    // Update the main Chicken
                    _context.Update(chicken);
                await _context.SaveChangesAsync(); // Save the updated Chicken

                    // Update offspring (if any)
                    if (chicken.Offspring.Any())
                {
                    foreach (var egg in chicken.Offspring)
                    {
                        egg.MotherChickenId = chicken.ChickenId; // Ensure the offspring is linked to the mother
                        _context.Update(egg); // Update each egg
                    }
                    await _context.SaveChangesAsync(); // Save all changes to the offspring
                }

                TempData["SuccessMessage"] = "Chicken updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating chicken: {ex.Message}";
                return View(chicken);
            }
        }

            // GET: Chickens/Delete/5
            [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Chicken ID not specified." });
            }

            try
            {
                var chicken = await _context.Chickens.FirstOrDefaultAsync(m => m.ChickenId == id);
                if (chicken == null)
                {
                    return RedirectToAction("Error", new { message = "chicken not found." });
                }

                return View(chicken);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading chicken for delete: {ex.Message}" });
            }
        }

            // POST: Chickens/Delete/5
            [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var chicken = await _context.Chickens.FindAsync(id);
                if (chicken == null)
                {
                    return RedirectToAction("Error", new { message = "Chicken not found." });
                }

                _context.Chickens.Remove(chicken);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Chicken deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting chicken: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ChickenExists(int id)
        {
            return _context.Chickens.Any(e => e.ChickenId == id);
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
