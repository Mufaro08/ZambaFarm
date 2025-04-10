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
            var farmContext = await _context.Turkeys.ToListAsync();
            return View(farmContext);
        }

        // GET: Turkeys/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Turkey());
        }

        // POST: Turkeys/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TurkeyId,TagNumber,Gender,BirthDate,IsEggLaying,NumberOfEggs,IsMating,MatingDate,MotherTurkeyId,MotherTagNumber")] Turkey turkey)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(turkey);
            }

            try
            {
                // Add the main turkey (mother)
                _context.Turkeys.Add(turkey);
                await _context.SaveChangesAsync(); // Save turkey first to get ID

                // Generate eggs if the turkey is laying eggs
                if (turkey.IsEggLaying && turkey.NumberOfEggs.HasValue)
                {
                    for (int i = 0; i < turkey.NumberOfEggs.Value; i++)
                    {
                        var egg = new Turkey
                        {
                            TagNumber = $"Egg-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                            Gender = "Unknown",
                            BirthDate = DateTime.Now,
                            MotherTagNumber = turkey.TagNumber,
                            MotherTurkeyId = turkey.TurkeyId
                        };
                        _context.Turkeys.Add(egg);
                    }
                    await _context.SaveChangesAsync(); // Save all eggs
                }

                TempData["SuccessMessage"] = "Turkey added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding turkey: {ex.Message}";
                return View(turkey);
            }
        }

        // GET: Turkeys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Turkey ID not specified." });
            }

            try
            {
                var turkey = await _context.Turkeys
                    .Include(t => t.Offspring)
                    .FirstOrDefaultAsync(t => t.TurkeyId == id);

                if (turkey == null)
                {
                    return RedirectToAction("Error", new { message = "Turkey not found." });
                }

                return View(turkey);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading turkey details: {ex.Message}" });
            }
        }

        // GET: Turkeys/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Turkey ID not specified." });
            }

            try
            {
                var turkey = await _context.Turkeys.FindAsync(id);
                if (turkey == null)
                {
                    return RedirectToAction("Error", new { message = "Turkey not found." });
                }
                return View(turkey);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading turkey for edit: {ex.Message}" });
            }
        }

        // POST: Turkeys/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TurkeyId,TagNumber,Gender,BirthDate,IsEggLaying,NumberOfEggs,IsMating,MatingDate,MotherTurkeyId,MotherTagNumber, Offspring")] Turkey turkey)
        {
            if (id != turkey.TurkeyId)
            {
                return RedirectToAction("Error", new { message = "Turkey ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(turkey);
            }

            try
            {
                // Update the main turkey
                _context.Update(turkey);
                await _context.SaveChangesAsync(); // Save the updated turkey

                // Update offspring (if any)
                if (turkey.Offspring.Any())
                {
                    foreach (var egg in turkey.Offspring)
                    {
                        egg.MotherTurkeyId = turkey.TurkeyId; // Ensure the offspring is linked to the mother
                        _context.Update(egg); // Update each egg
                    }
                    await _context.SaveChangesAsync(); // Save all changes to the offspring
                }

                TempData["SuccessMessage"] = "Turkey updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating turkey: {ex.Message}";
                return View(turkey);
            }
        }

        // GET: Turkeys/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Turkey ID not specified." });
            }

            try
            {
                var turkey = await _context.Turkeys.FirstOrDefaultAsync(m => m.TurkeyId == id);
                if (turkey == null)
                {
                    return RedirectToAction("Error", new { message = "Turkey not found." });
                }

                return View(turkey);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading turkey for delete: {ex.Message}" });
            }
        }

        // POST: Turkeys/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var turkey = await _context.Turkeys.FindAsync(id);
                if (turkey == null)
                {
                    return RedirectToAction("Error", new { message = "Turkey not found." });
                }

                _context.Turkeys.Remove(turkey);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Turkey deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting turkey: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TurkeyExists(int id)
        {
            return _context.Turkeys.Any(e => e.TurkeyId == id);
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
