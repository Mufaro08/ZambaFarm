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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pigs/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PigId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherPigId,MotherTagNumber")] Pig pig, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(pig);
            }

            try
            {
                // Handle image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(memoryStream);
                        pig.Image = memoryStream.ToArray(); // Store image as byte[]
                    }
                }
                else
                {
                    pig.Image = null; // Allow storing without an image
                }

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PigId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherPigId,MotherTagNumber, Offspring")] Pig pig, IFormFile imageFile )
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
                var existingPig = await _context.Pigs.FindAsync(id);
                if (existingPig == null)
                {
                    TempData["ErrorMessage"] = "Rabbit not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Update fields except image
                existingPig.TagNumber = pig.TagNumber;
                existingPig.Gender = pig.Gender;
                existingPig.BirthDate = pig.BirthDate;
                existingPig.IsPregnant = pig.IsPregnant;
                existingPig.IsNursing = pig.IsNursing;
                existingPig.IsMating = pig.IsMating;
                existingPig.MatingDate = pig.MatingDate;
                existingPig.NumberOfBabiesNursed = pig.NumberOfBabiesNursed;
                existingPig.MotherPigId = pig.MotherPigId;
                existingPig.MotherTagNumber = pig.MotherTagNumber;
                // 🖼️ Handle Image Upload - Only update if a new image is uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        existingPig.Image = memoryStream.ToArray(); // Store image as byte array
                    }
                }
                else
                {
                    imageFile = null;
                }

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
