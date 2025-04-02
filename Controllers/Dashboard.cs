using Microsoft.AspNetCore.Mvc;
using ZambaFarm.Models;

namespace FarmMonitor.Controllers
{

    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly FarmContext _context;

        public DashboardController(FarmContext context)
        {
            _context = context;
        }

        // 1. Get total count of each animal type
        [HttpGet("animalCounts")]
        public IActionResult GetAnimalCounts()
        {
            var data = new
            {
                Goats = _context.Goats.Count(),
                Pigs = _context.Pigs.Count(),
                Turkeys = _context.Turkeys.Count(),
                Rabbits = _context.Rabbits.Count()
            };

            return Ok(data);
        }

        // 2. Get monthly animal counts
        [HttpGet("monthlyAnimalCounts")]
        public IActionResult GetMonthlyAnimalCounts()
        {
            var monthlyData = _context.Goats
                .GroupBy(g => new { g.BirthDate.Year, g.BirthDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count(),
                    Species = "Goats"
                })
                .Union(_context.Pigs
                .GroupBy(p => new { p.BirthDate.Year, p.BirthDate.Month })
                .Select(p => new
                {
                    Year = p.Key.Year,
                    Month = p.Key.Month,
                    Count = p.Count(),
                    Species = "Pigs"
                }))
                .Union(_context.Turkeys
                .GroupBy(t => new { t.BirthDate.Year, t.BirthDate.Month })
                .Select(t => new
                {
                    Year = t.Key.Year,
                    Month = t.Key.Month,
                    Count = t.Count(),
                    Species = "Turkeys"
                }))
                .Union(_context.Rabbits
                .GroupBy(r => new { r.BirthDate.Year, r.BirthDate.Month })
                .Select(r => new
                {
                    Year = r.Key.Year,
                    Month = r.Key.Month,
                    Count = r.Count(),
                    Species = "Rabbits"
                }))
                .OrderBy(d => d.Year).ThenBy(d => d.Month)
                .ToList();

            return Ok(monthlyData);
        }
    }








    /*public class DashboardController : Controller
    {
        private readonly FarmContext _context;

        public DashboardController(FarmContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            var allRabbits = _context.Rabbits.ToList();
            var allPigs = _context.Pigs.ToList();
            var allCattles = _context.Cattles.ToList();
            var allGoats = _context.Goats.ToList();
            var allTurkeys = _context.Turkeys.ToList(); // Fetch data for turkeys

            // Aggregate data for the dashboard
            var totalRabbits = allRabbits.Count;
            var totalPigs = allPigs.Count;
            var totalCattles = allCattles.Count;
            var totalGoats = allGoats.Count;
            var totalTurkeys = allTurkeys.Count; // Aggregate data for turkeys

            ViewBag.TotalRabbits = totalRabbits;
            ViewBag.TotalPigs = totalPigs;
            ViewBag.TotalCattles = totalCattles;
            ViewBag.TotalGoats = totalGoats;
            ViewBag.TotalTurkeys = totalTurkeys; // Pass turkey data to the view

            return View();
        }
    }*/
}
