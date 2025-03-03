using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZambaFarm.Models
{
    public class Cattle
    {
        public int CattleId { get; set; }
        public string TagNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        [NotMapped]
        public int Age => (DateTime.Now - BirthDate).Days / 30; // Age in months

        public bool IsPregnant { get; set; }
        public bool IsNursing { get; set; }
        public bool IsMating { get; set; }
        public DateTime? MatingDate { get; set; }

        [NotMapped]
        public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue ? MatingDate.Value.AddDays(30) : (DateTime?)null;

        [NotMapped]
        public string Status => Gender == "Female"
            ? (IsNursing ? "Nursing" : (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None")))
            : "None";

        // Self-referencing Relationship
        public int? MotherId { get; set; }

        [ForeignKey("MotherId")]
        [InverseProperty("Offspring")]
        public virtual Cattle Mother { get; set; }

        public virtual ICollection<Cattle> Offspring { get; set; } = new List<Cattle>();
    }
}
