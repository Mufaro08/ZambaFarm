using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class Turkey
    {
        public int TurkeyId { get; set; }

        [Required(ErrorMessage = "Tag Number is required.")]
        public string TagNumber { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Is Mated")]
        public bool IsMated { get; set; }

        public int? NumberOfEggsLaid { get; set; }  // Nullable to track number of eggs laid by turkey

        public DateTime? MatingDate { get; set; }

        public int? MotherTurkeyId { get; set; }
        public string? MotherTagNumber { get; set; }

        public bool IsEggLaying { get; set; }

        public int? NumberOfEggs { get; set; } // Nullable to handle the number of eggs properly

        public DateTime? EggLayingDate => IsMated && MatingDate.HasValue
            ? MatingDate.Value.AddDays(28)  // Turkeys have a 28-day incubation period for eggs
            : (DateTime?)null;

        public string Status => Gender == "Female"
            ? (IsMated ? "Mated" : "None")
            : "None";

        public virtual ICollection<Turkey> Offspring { get; set; } = new List<Turkey>();

        [BindNever]
        public virtual Turkey? Mother { get; set; }

        public void AddEggs()
        {
            if (IsMated && NumberOfEggsLaid.HasValue)
            {
                for (int i = 0; i < NumberOfEggsLaid.Value; i++)
                {
                    Offspring.Add(new Turkey
                    {
                        TagNumber = $"Egg-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
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
