using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class Pig
    {
        public int PigId { get; set; }

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

        public int? NumberOfPiglets { get; set; }  // Tracks number of piglets

        public bool IsMating { get; set; }
        public DateTime? MatingDate { get; set; }

        public int? MotherPigId { get; set; }
        public string? MotherTagNumber { get; set; }

        [Display(Name = "Is Nursing")]
        public bool IsNursing { get; set; }

        public int? NumberOfBabiesNursed { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue
            ? MatingDate.Value.AddDays(114)  // Pigs have a gestation period of 114 days
            : (DateTime?)null;

        public string Status => Gender == "Female"
            ? (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None"))
            : "None";

        public virtual ICollection<Pig> Offspring { get; set; } = new List<Pig>();

        [BindNever]
        public virtual Pig? Mother { get; set; }
       
        public void AddPiglets()
        {
            if (IsNursing && NumberOfBabiesNursed.HasValue)
            {
                for (int i = 0; i < NumberOfBabiesNursed.Value; i++)
                {
                    Offspring.Add(new Pig
                    {
                        TagNumber = $"Piglet-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
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
