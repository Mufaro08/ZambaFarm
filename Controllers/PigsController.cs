using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

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
            var farmContext = await _context.Pigs.ToListAsync();
            return View(farmContext);
        }

        // GET: Pigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pigs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PigId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherPigId,MotherTagNumber")] Pig pig)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(pig);
            }

            try
            {
               

                // Add the main pig (mother)
                _context.Pigs.Add(pig);
                await _context.SaveChangesAsync(); // Save pig first to get ID

                // Generate offspring if nursing
                pig.AddPiglets();

                // Explicitly add offspring to the database
                if (pig.Offspring.Any())
                {
                    foreach (var baby in pig.Offspring)
                    {
                        baby.MotherPigId = pig.PigId; // Set mother's ID
                        _context.Pigs.Add(baby); // Add each baby to the database
                    }
                    await _context.SaveChangesAsync(); // Save all offspring
                }

                TempData["SuccessMessage"] = "Pig added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding Pig: {ex.Message}";
                return View(pig);
            }
        }

        // GET: Pigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Pig ID not specified." });
            }

            try
            {
                var pig = await _context.Pigs
                    .Include(r => r.Offspring) // If you need to include offspring
                    .FirstOrDefaultAsync(r => r.PigId == id);

                if (pig == null)
                {
                    return RedirectToAction("Error", new { message = "Pig not found." });
                }

                return View(pig);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading pig details: {ex.Message}" });
            }
        }


        // GET: pigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Pig ID not specified." });
            }

            try
            {
                var pig = await _context.Pigs.FindAsync(id);
                if (pig == null)
                {
                    return RedirectToAction("Error", new { message = "Pig not found." });
                }
                return View(pig);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading pig for edit: {ex.Message}" });
            }
        }

        // POST: Pigs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PigId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherPigId,MotherTagNumber, Offspring")] Pig pig )
        {
            if (id != pig.PigId)
            {
                return RedirectToAction("Error", new { message = "Pig ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(pig);
            }

            try
            {
                // Check if the pig is nursing and generate offspring
                if (pig.IsNursing)
                {
                    pig.AddPiglets();
                }

                // Update the main pig
                _context.Update(pig);
                await _context.SaveChangesAsync(); // Save the updated pig

                // Update offspring (if any)
                if (pig.Offspring.Any())
                {
                    foreach (var baby in pig.Offspring)
                    {
                        baby.MotherPigId = pig.PigId; // Ensure the offspring is linked to the mother
                        _context.Update(baby); // Update each baby
                    }
                    await _context.SaveChangesAsync(); // Save all changes to the offspring
                }

                TempData["SuccessMessage"] = "Pig updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating pig: {ex.Message}";
                return View(pig);
            }
        }

        // GET: Pigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Pig ID not specified." });
            }

            try
            {
                var pig = await _context.Pigs.FirstOrDefaultAsync(m => m.PigId == id);
                if (pig == null)
                {
                    return RedirectToAction("Error", new { message = "Pig not found." });
                }

                return View(pig);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading pig for delete: {ex.Message}" });
            }
        }

        // POST: Pigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var pig = await _context.Pigs.FindAsync(id);
                if (pig == null)
                {
                    return RedirectToAction("Error", new { message = "Pig not found." });
                }

                _context.Pigs.Remove(pig);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pig deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting pig: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PigExists(int id)
        {
            return _context.Pigs.Any(e => e.PigId == id);
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
