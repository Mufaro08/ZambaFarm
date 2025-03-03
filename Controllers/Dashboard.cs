

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
            // Add code to fetch data for other species as well

            // Aggregate data for the dashboard
            var totalRabbits = allRabbits.Count;
            var totalPigs = allPigs.Count;
            var totalCattles = allCattles.Count;
            var totalGoats = allGoats.Count;
            // Add aggregation logic for other species

            ViewBag.TotalRabbits = totalRabbits;
            ViewBag.TotalPigs = totalPigs;
            ViewBag.TotalCattles = totalCattles;
            ViewBag.TotalGoats = totalGoats;
            // Pass aggregated data to the view

            return View();
        }
    }
}
