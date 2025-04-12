using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;

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
            var farmContext = await _context.Rabbits.ToListAsync();
            return View(farmContext);
        }

        // GET: Rabbits/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rabbits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,Cage,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherRabbitId,MotherTagNumber")] Rabbit rabbit, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(rabbit);
            }

            try
            {
                // Handle image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(memoryStream);
                        rabbit.Image = memoryStream.ToArray(); // Store image as byte[]
                    }
                }
                else
                {
                    rabbit.Image = null; // Allow storing without an image
                }

                // Add the main rabbit (mother)
                _context.Rabbits.Add(rabbit);
                await _context.SaveChangesAsync(); // Save rabbit first to get ID

                // Generate offspring if nursing
                rabbit.AddNursedBabies();

                // Explicitly add offspring to the database
                if (rabbit.Offspring.Any())
                {
                    foreach (var baby in rabbit.Offspring)
                    {
                        baby.MotherRabbitId = rabbit.RabbitId; // Set mother's ID
                        _context.Rabbits.Add(baby); // Add each baby to the database
                    }
                    await _context.SaveChangesAsync(); // Save all offspring
                }

                TempData["SuccessMessage"] = "Rabbit added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding rabbit: {ex.Message}";
                return View(rabbit);
            }
        }

        // GET: Rabbits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Rabbit ID not specified." });
            }

            try
            {
                var rabbit = await _context.Rabbits
                    .Include(r => r.Offspring) // If you need to include offspring
                    .FirstOrDefaultAsync(r => r.RabbitId == id);

                if (rabbit == null)
                {
                    return RedirectToAction("Error", new { message = "Rabbit not found." });
                }

                return View(rabbit);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading rabbit details: {ex.Message}" });
            }
        }

        // GET: Rabbits/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Rabbit ID not specified." });
            }

            try
            {
                var rabbit = await _context.Rabbits.FindAsync(id);
                if (rabbit == null)
                {
                    return RedirectToAction("Error", new { message = "Rabbit not found." });
                }
                return View(rabbit);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading rabbit for edit: {ex.Message}" });
            }
        }

        //  GET: Rabbits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,Cage,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherRabbitId,MotherTagNumber")] Rabbit rabbit, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(rabbit);
            }

            try
            {
                var existingRabbit = await _context.Rabbits.FindAsync(id);
                if (existingRabbit == null)
                {
                    TempData["ErrorMessage"] = "Rabbit not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Update fields except image
                existingRabbit.TagNumber = rabbit.TagNumber;
                existingRabbit.Gender = rabbit.Gender;
                existingRabbit.BirthDate = rabbit.BirthDate;
                existingRabbit.IsPregnant = rabbit.IsPregnant;
                existingRabbit.Cage = rabbit.Cage;
                existingRabbit.IsNursing = rabbit.IsNursing;
                existingRabbit.IsMating = rabbit.IsMating;
                existingRabbit.MatingDate = rabbit.MatingDate;
                existingRabbit.NumberOfBabiesNursed = rabbit.NumberOfBabiesNursed;
                existingRabbit.MotherRabbitId = rabbit.MotherRabbitId;
                existingRabbit.MotherTagNumber = rabbit.MotherTagNumber;

                // 🖼️ Handle Image Upload - Only update if a new image is uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        existingRabbit.Image = memoryStream.ToArray(); // Store image as byte array
                    }
                }

                _context.Update(existingRabbit);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Rabbit updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating rabbit: {ex.Message}";
                return View(rabbit);
            }
        }






        /* // GET: Rabbits/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Rabbit ID not specified." });
            }

            try
            {
                var rabbit = await _context.Rabbits.FindAsync(id);
                if (rabbit == null)
                {
                    return RedirectToAction("Error", new { message = "Rabbit not found." });
                }
                return View(rabbit);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading rabbit for edit: {ex.Message}" });
            }
        }

        // POST: Rabbits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherRabbitId,MotherTagNumber, Offspring")] Rabbit rabbit)
        {
            if (id != rabbit.RabbitId)
            {
                return RedirectToAction("Error", new { message = "Rabbit ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
                return View(rabbit);
            }

            try
            {
                // Check if the rabbit is nursing and generate offspring
                if (rabbit.IsNursing)
                {
                    rabbit.AddNursedBabies();
                }

                // Update the main rabbit
                _context.Update(rabbit);
                await _context.SaveChangesAsync(); // Save the updated rabbit

                // Update offspring (if any)
                if (rabbit.Offspring.Any())
                {
                    foreach (var baby in rabbit.Offspring)
                    {
                        baby.MotherRabbitId = rabbit.RabbitId; // Ensure the offspring is linked to the mother
                        _context.Update(baby); // Update each baby
                    }
                    await _context.SaveChangesAsync(); // Save all changes to the offspring
                }

                TempData["SuccessMessage"] = "Rabbit updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating rabbit: {ex.Message}";
                return View(rabbit);
            }
        }
        */

        // GET: Rabbits/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", new { message = "Rabbit ID not specified." });
            }

            try
            {
                var rabbit = await _context.Rabbits.FirstOrDefaultAsync(m => m.RabbitId == id);
                if (rabbit == null)
                {
                    return RedirectToAction("Error", new { message = "Rabbit not found." });
                }

                return View(rabbit);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading rabbit for delete: {ex.Message}" });
            }
        }

        // POST: Rabbits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var rabbit = await _context.Rabbits.FindAsync(id);
                if (rabbit == null)
                {
                    return RedirectToAction("Error", new { message = "Rabbit not found." });
                }

                _context.Rabbits.Remove(rabbit);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rabbit deleted successfully!";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error deleting rabbit: {ex.Message}" });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RabbitExists(int id)
        {
            return _context.Rabbits.Any(e => e.RabbitId == id);
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
