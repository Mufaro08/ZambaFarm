using Microsoft.AspNetCore.Mvc;
using ZambaFarm.Models;

namespace FarmMonitor.Controllers
{
    public class DashboardController : Controller
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
    }
}
