using Microsoft.AspNetCore.Mvc;
using ZambaFarm.Models;

namespace FarmMonitor.Controllers
{
    public class DashboardController : Controller
    {
        private FarmContext db = new FarmContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            var allRabbits = db.Rabbits.ToList();
            var allPigs = db.Pigs.ToList();
            var allCattles = db.Cattles.ToList();
            var allGoats = db.Goats.ToList();
            var allTurkeys = db.Turkeys.ToList(); // Fetch data for turkeys

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
