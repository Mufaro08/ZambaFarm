using System;
using System.Collections.Generic;

namespace ZambaFarm.Models
{
    public class Rabbit
    {
        public int RabbitId { get; set; }
        public string TagNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age => (DateTime.Now - BirthDate).Days / 30; // Age in months
        public bool IsPregnant { get; set; }
        public bool IsNursing { get; set; }
        public bool IsMating { get; set; }
        public DateTime? MatingDate { get; set; }
        public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue ? MatingDate.Value.AddDays(30) : (DateTime?)null;
        public string Status => Gender == "Female" ? (IsNursing ? "Nursing" : (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None"))) : "None";
        public virtual ICollection<Rabbit> Offspring { get; set; }
        public int? MotherId { get; set; }
        public virtual Rabbit Mother { get; set; }
    }
}
