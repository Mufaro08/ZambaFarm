using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class Rabbit
    {
        public int RabbitId { get; set; }

        [Required(ErrorMessage = "Tag Number is required.")]
        public string TagNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime BirthDate { get; set; }

        public int Age => (DateTime.Now - BirthDate).Days / 30; // Age in months

        // Apply conditional logic for IsPregnant
        [Display(Name = "Is Pregnant")]
        public bool IsPregnant
        {
            get => Gender == "Female" ? _isPregnant : false;
            set => _isPregnant = Gender == "Female" ? value : false;
        }
        private bool _isPregnant;

        // Apply conditional logic for IsNursing
        [Display(Name = "Is Nursing")]
        public bool IsNursing
        {
            get => Gender == "Female" ? _isNursing : false;
            set => _isNursing = Gender == "Female" ? value : false;
        }
        private bool _isNursing;

        public bool IsMating { get; set; }
        public DateTime? MatingDate { get; set; }

        public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue
            ? MatingDate.Value.AddDays(30)
            : (DateTime?)null;

        public string Status => Gender == "Female"
            ? (IsNursing ? "Nursing" : (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None")))
            : "None";

        public virtual ICollection<Rabbit> Offspring { get; set; }
        public int? MotherId { get; set; }
        public virtual Rabbit Mother { get; set; }
    }
}
