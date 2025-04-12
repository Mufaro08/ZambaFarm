using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class Goat
    {
        public int GoatId { get; set; }

        [Required(ErrorMessage = "Tag Number is required.")]
        public string TagNumber { get; set; }

        public byte[]? Image { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Is Pregnant")]
        public bool IsPregnant { get; set; }

        public int? NumberOfKids { get; set; }  // Tracks number of kids born

        public bool IsMating { get; set; }
        public DateTime? MatingDate { get; set; }

        public int? MotherGoatId { get; set; }
        public string? MotherTagNumber { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue
            ? MatingDate.Value.AddDays(150)  // Goats have a gestation period of about 150 days
            : (DateTime?)null;

        public string Status => Gender == "Female"
            ? (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None"))
            : "None";

        public virtual ICollection<Goat> Offspring { get; set; } = new List<Goat>();

        [BindNever]
        public virtual Goat? Mother { get; set; }
        
        [Display(Name = "Is Nursing")]
        public bool IsNursing { get; set; }

        public int? NumberOfBabiesNursed { get; set; }

        public void AddNursedKids()
        {
            if (IsNursing && NumberOfBabiesNursed.HasValue)
            {
                for (int i = 0; i < NumberOfBabiesNursed.Value; i++)
                {
                    Offspring.Add(new Goat
                    {                        
                        TagNumber = $"Kid-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
                        Gender = "Unknown",
                        BirthDate = DateTime.Now,
                        MotherTagNumber = this.TagNumber,
                        Mother = this
                    });
                }
            }
        }
    }
}
