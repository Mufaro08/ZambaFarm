using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class Cattle
    {
        public int CattleId { get; set; }

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

        public int? NumberOfCalves { get; set; }  // Tracks number of calves born

        public bool IsMating { get; set; }
        public DateTime? MatingDate { get; set; }

        public int? MotherCattleId { get; set; }
        public string? MotherTagNumber { get; set; }

        [Display(Name = "Is Nursing")]
        public bool IsNursing { get; set; }

        public int? NumberOfBabiesNursed { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue
            ? MatingDate.Value.AddDays(280)  // Cattle pregnancy lasts around 280 days
            : (DateTime?)null;

        public string Status => Gender == "Female"
            ? (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None"))
            : "None";

        public virtual ICollection<Cattle> Offspring { get; set; } = new List<Cattle>();

        [BindNever]
        public virtual Cattle? Mother { get; set; }  // Navigation property

        public void AddCalves()
        {
            if (IsNursing && NumberOfBabiesNursed.HasValue)
            {
                for (int i = 0; i < NumberOfBabiesNursed.Value; i++)
                {
                    Offspring.Add(new Cattle
                    {
                        TagNumber = $"Calf-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
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
