using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZambaFarm.Data;
using System.Data;

namespace ZambaFarm.Models
{
    public class DashboardViewModel
    {
        public object TotalAnimals { get; set; }
        public object AnimalGrowth { get; set; }
    }

    public class AnimalCount
    {
        public int Rabbits { get; set; }
        public int Pigs { get; set; }
        public int Cattles { get; set; }
        public int Goats { get; set; }
        public int Turkeys { get; set; }
    }

    public class AnimalGrowth
    {
        public List<SpeciesGrowth> Rabbits { get; set; }
        public List<SpeciesGrowth> Pigs { get; set; }
        public List<SpeciesGrowth> Cattles { get; set; }
        public List<SpeciesGrowth> Goats { get; set; }
        public List<SpeciesGrowth> Turkeys { get; set; }
    }

    public class SpeciesGrowth
    {
        public int Month { get; set; }
        public int Count { get; set; }
    }
}

