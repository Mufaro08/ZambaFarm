using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class Chicken
    {
        public int ChickenId { get; set; }

        [Required(ErrorMessage = "Tag Number is required.")]
        public string TagNumber { get; set; }

        public byte[]? Image { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Is Mated")]
        public bool IsMated { get; set; }

        public int? NumberOfEggsLaid { get; set; }  // Nullable to track number of eggs laid by Chicken

        public DateTime? MatingDate { get; set; }

        public int? MotherChickenId { get; set; }
        public string? MotherTagNumber { get; set; }

        public bool IsEggLaying { get; set; }
        public int? IsNursing { get; set; }

        public int? NumberOfEggs { get; set; } // Nullable to handle the number of eggs properly

        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime? EggLayingDate => IsMated && MatingDate.HasValue
            ? MatingDate.Value.AddDays(28)  // Chickens have a 28-day incubation period for eggs
            : (DateTime?)null;

        public string Status => Gender == "Female"
            ? (IsMated ? "Mated" : "None")
            : "None";

        public virtual ICollection<Chicken> Offspring { get; set; } = new List<Chicken>();

        [BindNever]
        public virtual Chicken? Mother { get; set; }

        public void AddEggs()
        {
            if (IsMated && NumberOfEggsLaid.HasValue)
            {
                for (int i = 0; i < NumberOfEggsLaid.Value; i++)
                {
                    Offspring.Add(new Chicken
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
