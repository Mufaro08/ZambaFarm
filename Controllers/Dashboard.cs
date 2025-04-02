using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZambaFarm.Models;

namespace ZambaFarm.Controllers
{
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly FarmContext _context;

        public DashboardController(FarmContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get total counts for each animal type
            var animalCounts = new Dictionary<string, int>
            {
                { "Pigs", await _context.Pigs.CountAsync() },
                { "Goats", await _context.Goats.CountAsync() },
                { "Turkeys", await _context.Turkeys.CountAsync() },
                { "Rabbits", await _context.Rabbits.CountAsync() }
            };

            // Get monthly count for each animal type
            var startDate = DateTime.Now.AddMonths(-6); // Last 6 months
            var monthlyCounts = await _context.Pigs
                .Where(p => p.BirthDate >= startDate)
                .GroupBy(p => new { p.BirthDate.Year, p.BirthDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            var goatMonthlyCounts = await _context.Goats
                .Where(g => g.BirthDate >= startDate)
                .GroupBy(g => new { g.BirthDate.Year, g.BirthDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            var turkeyMonthlyCounts = await _context.Turkeys
                .Where(t => t.BirthDate >= startDate)
                .GroupBy(t => new { t.BirthDate.Year, t.BirthDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            var rabbitMonthlyCounts = await _context.Rabbits
                .Where(r => r.BirthDate >= startDate)
                .GroupBy(r => new { r.BirthDate.Year, r.BirthDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            ViewData["AnimalCounts"] = animalCounts;
            ViewData["MonthlyData"] = new Dictionary<string, List<object>>
            {
                { "Pigs", monthlyCounts.Cast<object>().ToList() },
                { "Goats", goatMonthlyCounts.Cast<object>().ToList() },
                { "Turkeys", turkeyMonthlyCounts.Cast<object>().ToList() },
                { "Rabbits", rabbitMonthlyCounts.Cast<object>().ToList() }
            };

            return View();
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
