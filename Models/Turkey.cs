using System;

namespace ZambaFarm.Models
{
    public class Turkey
    {
        public int TurkeyId { get; set; } // Unique identifier for the turkey
        public string TagNumber { get; set; } // Tag number for identifying the turkey
        public string Gender { get; set; } // Male or Female
        public DateTime BirthDate { get; set; } // Date of birth
        public int Age => (DateTime.Now - BirthDate).Days / 30; // Age in months
        public bool IsBreeding { get; set; } // Indicates if the turkey is used for breeding
        public DateTime? BreedingDate { get; set; } // Date when breeding occurred
        public DateTime? NextBreedingDate => BreedingDate.HasValue ? BreedingDate.Value.AddMonths(2) : (DateTime?)null; // Suggested next breeding date
        public string Status => IsBreeding ? "Breeding" : "Available"; // Breeding or available status
        public double Weight { get; set; } // Weight in kilograms
        public virtual ICollection<Turkey> Offspring { get; set; }
        public int? MotherId { get; set; }
        public virtual Turkey Mother { get; set; }
    }
}
