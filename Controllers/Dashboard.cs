 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZambaFarm.Models;

namespace ZambaFarm.Controllers
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
            var allTurkeys = _context.Turkeys.ToList();
            var allChickens = _context.Chickens.ToList();
            var allDucks = _context.Ducks.ToList();

            // Aggregate data for the dashboard
            var totalRabbits = allRabbits.Count;
            var totalPigs = allPigs.Count;
            var totalCattles = allCattles.Count;
            var totalGoats = allGoats.Count;
            var totalTurkeys = allTurkeys.Count;
            var totalChickens = allChickens.Count;
            var totalDucks = allDucks.Count;

            ViewBag.TotalRabbits = totalRabbits;
            ViewBag.TotalPigs = totalPigs;
            ViewBag.TotalCattles = totalCattles;
            ViewBag.TotalGoats = totalGoats;
            ViewBag.TotalTurkeys = totalTurkeys;
            ViewBag.TotalChickens = totalChickens;
            ViewBag.TotalDucks = totalDucks;

            return View();
        }
    }

}