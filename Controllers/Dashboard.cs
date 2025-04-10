using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZambaFarm.Data;
using ZambaFarm.Models;

[Route("Dashboard")]
public class DashboardController : Controller
{
    private readonly FarmContext _context;

    public DashboardController(FarmContext context)
    {
        _context = context;
    }

    // Serve the Dashboard Index View
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // API for Animal Counts
    [HttpGet("api/animalCounts")]
    public IActionResult GetAnimalCounts()
    {
        var animalCounts = new
        {
            Goats = _context.Goats.Count(),
            Pigs = _context.Pigs.Count(),
            Cows = _context.Cattles.Count(),
            Rabbits = _context.Rabbits.Count(),
            Turkeys = _context.Turkeys.Count()
        };

        return Ok(animalCounts);
    }

    [HttpGet("api/monthlyAnimalCounts")]
    public IActionResult GetMonthlyAnimalCounts()
    {
        var monthlyCounts = _context.Goats
    .GroupBy(a => new { Year = a.DateAdded.Year, Month = a.DateAdded.Month })
    .Select(g => new
    {
        Species = "Goats",
        Year = g.Key.Year,
        Month = g.Key.Month,
        Count = g.Count()
    })
    .Union(
        _context.Pigs.GroupBy(a => new { Year = a.DateAdded.Year, Month = a.DateAdded.Month })
        .Select(g => new
        {
            Species = "Pigs",
            Year = g.Key.Year,
            Month = g.Key.Month,
            Count = g.Count()
        })
    )
    .Union(
        _context.Cattles.GroupBy(a => new { Year = a.DateAdded.Year, Month = a.DateAdded.Month })
        .Select(g => new
        {
            Species = "Cows",
            Year = g.Key.Year,
            Month = g.Key.Month,
            Count = g.Count()
        })
    )
    .Union(
        _context.Rabbits.GroupBy(a => new { Year = a.DateAdded.Year, Month = a.DateAdded.Month })
        .Select(g => new
        {
            Species = "Rabbits",
            Year = g.Key.Year,
            Month = g.Key.Month,
            Count = g.Count()
        })
    )
    .Union(
        _context.Turkeys.GroupBy(a => new { Year = a.DateAdded.Year, Month = a.DateAdded.Month })
        .Select(g => new
        {
            Species = "Turkeys",
            Year = g.Key.Year,
            Month = g.Key.Month,
            Count = g.Count()
        })
    )
    .ToList();


        return Ok(monthlyCounts);
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
