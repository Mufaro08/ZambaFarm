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
    public class GoatsController : Controller
    {
        private readonly FarmContext _context;

        public GoatsController(FarmContext context)
        {
            _context = context;
        }

        // GET: Goats
        public async Task<IActionResult> Index()
        {
            var farmContext = await _context.Goats.Include(g => g.Offspring).ToListAsync();
            return View(farmContext);
        }

        // GET: Goats/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Goats/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GoatId,TagNumber,Gender,BirthDate,IsNursing,NumberOfBabiesNursed,MotherGoatId,MotherTagNumber")] Goat goat)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Validation failed.";
                return View(goat);
            }

            try
            {
                _context.Goats.Add(goat);
                await _context.SaveChangesAsync();

                // Generate offspring if nursing
               // goat.AddNursedKids();

                // If goat is nursing, add kids
                if (goat.IsNursing && goat.NumberOfBabiesNursed.HasValue)
                {
                    for (int i = 0; i < goat.NumberOfBabiesNursed.Value; i++)
                    {
                        var kid = new Goat
                        {
                            TagNumber = $"Kid-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                            Gender = "Unknown",
                            BirthDate = DateTime.Now,
                            MotherTagNumber = goat.TagNumber,
                            MotherGoatId = goat.GoatId
                        };

                        _context.Goats.Add(kid);
                        Console.WriteLine($"Added Kid: {kid.TagNumber} for Mother: {goat.TagNumber}"); // Debugging Log
                    }
                    await _context.SaveChangesAsync();
                }


                TempData["SuccessMessage"] = "Goat added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding goat: {ex.Message}";
                return View(goat);
            }
        }

        // GET: Goats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Goat ID not specified." });
            }

            var goat = await _context.Goats
                .Include(g => g.Offspring) // Ensure offspring are included
                .FirstOrDefaultAsync(m => m.GoatId == id);

            if (goat == null)
            {
                return RedirectToAction("Error", new { message = "Goat not found." });
            }

            return View(goat);
        }


        // GET: Goats/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Error", new { message = "Goat ID not specified." });

            var goat = await _context.Goats.FindAsync(id);
            if (goat == null)
                return RedirectToAction("Error", new { message = "Goat not found." });

            return View(goat);
        }

        // POST: Goats/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GoatId,TagNumber,Gender,BirthDate,IsNursing,NumberOfBabiesNursed,MotherGoatId,MotherTagNumber")] Goat goat)
        {
            if (id != goat.GoatId)
                return RedirectToAction("Error", new { message = "Goat ID mismatch." });

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Validation failed.";
                return View(goat);
            }

            try
            {
                _context.Update(goat);
                await _context.SaveChangesAsync();

                // Update offspring if needed
                if (goat.IsNursing && goat.NumberOfBabiesNursed.HasValue)
                {
                    for (int i = 0; i < goat.NumberOfBabiesNursed.Value; i++)
                    {
                        var kid = new Goat
                        {
                            TagNumber = $"Kid-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                            Gender = "Unknown",
                            BirthDate = DateTime.Now,
                            MotherTagNumber = goat.TagNumber,
                            MotherGoatId = goat.GoatId
                        };

                        _context.Goats.Add(kid);
                        Console.WriteLine($"Added Kid: {kid.TagNumber} for Mother: {goat.TagNumber}"); // Debugging Log
                    }
                    await _context.SaveChangesAsync();
                }


                TempData["SuccessMessage"] = "Goat updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating goat: {ex.Message}";
                return View(goat);
            }
        }

        // GET:Goats/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Goat ID not specified." });
            }

            try
            {
                var goat = await _context.Goats.FirstOrDefaultAsync(m => m.GoatId == id);
                if (goat == null)
                {
                    return RedirectToAction("Error", new { message = "Goat not found." });
                }

                return View(goat);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading goat for delete: {ex.Message}" });
            }
        }

        // POST: Goats/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var goat = await _context.Goats.FindAsync(id);
                if (goat == null)
                {
                    return RedirectToAction("Error", new { message = "Goat not found." });
                }

                _context.Goats.Remove(goat);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Goat deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting goat: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
