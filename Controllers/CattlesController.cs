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
    public class CattlesController : Controller
    {
        private readonly FarmContext _context;

        public CattlesController(FarmContext context)
        {
            _context = context;
        }

        // GET: Cattles
        public async Task<IActionResult> Index()
        {
            var farmContext = await _context.Cattles.Include(c => c.Offspring).ToListAsync();
            return View(farmContext);
        }

        // GET: Cattles/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new Cattle());
        }

        // POST: Cattles/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CattleId,TagNumber,Gender,BirthDate,IsPregnant,NumberOfBabiesNursed,IsMating,MatingDate,MotherCattleId,MotherTagNumber")] Cattle cattle, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(cattle);
            }

            try
            {
                // Handle image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(memoryStream);
                        cattle.Image = memoryStream.ToArray(); // Store image as byte[]
                    }
                }
                else
                {
                    cattle.Image = null; // Allow storing without an image
                }

                _context.Cattles.Add(cattle);
                await _context.SaveChangesAsync();

                if (cattle.IsNursing && cattle.NumberOfBabiesNursed.HasValue)
                {
                    for (int i = 0; i < cattle.NumberOfBabiesNursed.Value; i++)
                    {
                        var baby = new Cattle
                        {
                            TagNumber = $"Calf-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                            Gender = "Unknown",
                            BirthDate = DateTime.Now,
                            MotherTagNumber = cattle.TagNumber,
                            MotherCattleId = cattle.CattleId
                        };
                        _context.Cattles.Add(baby);
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Cattle added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding cattle: {ex.Message}";
                return View(cattle);
            }
        }

        // GET: Cattle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Cattle ID not specified." });
            }

            try
            {
                var cattle = await _context.Cattles
                    .Include(c => c.Offspring) // If you need to include offspring
                    .FirstOrDefaultAsync(c => c.CattleId == id);

                if (cattle == null)
                {
                    return RedirectToAction("Error", new { message = "Cattle not found." });
                }

                return View(cattle);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading cattle details: {ex.Message}" });
            }
        }


        // GET: Cattle/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Cattle ID not specified." });
            }

            try
            {
                var cattle = await _context.Cattles.FindAsync(id);
                if (cattle == null)
                {
                    return RedirectToAction("Error", new { message = "Cattle not found." });
                }
                return View(cattle);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading cattle for edit: {ex.Message}" });
            }
        }

        // POST: Cattle/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CattletId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherCattleId,MotherTagNumber, Offspring")] Cattle cattle)
        {
            if (id != cattle.CattleId)
            {
                return RedirectToAction("Error", new { message = "Cattle ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(cattle);
            }

            try
            {
                // Check if the cattle is nursing and generate offspring
                if (cattle.IsNursing)
                {
                    cattle.AddCalves();
                }

                // Update the main cattle
                _context.Update(cattle);
                await _context.SaveChangesAsync(); // Save the updated cattle

                // Update offspring (if any)
                if (cattle.Offspring.Any())
                {
                    foreach (var baby in cattle.Offspring)
                    {
                        baby.MotherCattleId = cattle.CattleId; // Ensure the offspring is linked to the mother
                        _context.Update(baby); // Update each baby
                    }
                    await _context.SaveChangesAsync(); // Save all changes to the offspring
                }

                TempData["SuccessMessage"] = "Cattle updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating cattle: {ex.Message}";
                return View(cattle);
            }
        }

        // GET: Cattle/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Cattle ID not specified." });
            }

            try
            {
                var cattle = await _context.Cattles.FirstOrDefaultAsync(m => m.CattleId == id);
                if (cattle == null)
                {
                    return RedirectToAction("Error", new { message = "Cattle not found." });
                }

                return View(cattle);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading cattle for delete: {ex.Message}" });
            }
        }

        // POST: Cattle/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cattle = await _context.Cattles.FindAsync(id);
                if (cattle == null)
                {
                    return RedirectToAction("Error", new { message = "Cattle not found." });
                }

                _context.Cattles.Remove(cattle);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cattle deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting cattle: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CattleExists(int id)
        {
            return _context.Cattles.Any(e => e.CattleId == id);
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
