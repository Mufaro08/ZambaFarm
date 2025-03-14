using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using System;
using System.Diagnostics;
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
            try
            {
                var farmContext = await _context.Rabbits.ToListAsync();
                return View(farmContext);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading rabbits: {ex.Message}" });
            }
        }

        // GET: Rabbits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rabbits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RabbitId,TagNumber,Gender,BirthDate,IsPregnant,IsNursing,IsMating,MatingDate,NumberOfBabiesNursed,MotherRabbitTag")] Rabbit rabbit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rabbit.AddNursedBabies();
                    foreach (var baby in rabbit.Offspring)
                    {
                        baby.MotherTagNumber = rabbit.TagNumber;
                    }

                    _context.Add(rabbit);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rabbit added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = $"Error adding rabbit: {ex.Message}" });
                }
            }

            return View(rabbit);
        }

        // GET: Rabbits/Edit/5
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
                    .Include(r => r.Offspring)
                    .FirstOrDefaultAsync(r => r.RabbitId == id);

                if (rabbit == null)
                {
                    return RedirectToAction("Error", new { message = "Rabbit not found." });
                }

                if (!string.IsNullOrEmpty(rabbit.MotherTagNumber))
                {
                    var mother = await _context.Rabbits.FirstOrDefaultAsync(r => r.TagNumber == rabbit.MotherTagNumber);
                    if (mother != null)
                    {
                        ViewData["MotherDetails"] = mother;
                    }
                }

                return View(rabbit);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = $"Error loading rabbit details: {ex.Message}" });
            }
        }

        // GET: Rabbits/Delete/5
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
