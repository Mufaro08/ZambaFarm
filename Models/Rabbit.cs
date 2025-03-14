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

    [Display(Name = "Is Pregnant")]
    public bool IsPregnant { get; set; }

    [Display(Name = "Is Nursing")]
    public bool IsNursing { get; set; }

    public int? NumberOfBabiesNursed { get; set; }
    public bool IsMating { get; set; }
    public DateTime? MatingDate { get; set; }

    public DateTime? DeliveryDate => IsPregnant && MatingDate.HasValue
        ? MatingDate.Value.AddDays(30)
        : (DateTime?)null;

    public string Status => Gender == "Female"
        ? (IsNursing ? "Nursing" : (IsPregnant ? "Pregnant" : (IsMating ? "Mating" : "None")))
        : "None";

    public virtual ICollection<Rabbit> Offspring { get; set; } = new List<Rabbit>();

    // New Properties
    public string MotherTagNumber { get; set; } // Tag of the mother
    public virtual Rabbit Mother { get; set; } // Navigation property to the mother
       

        public void AddNursedBabies()
    {
        if (IsNursing && NumberOfBabiesNursed.HasValue)
        {
            for (int i = 0; i < NumberOfBabiesNursed.Value; i++)
            {
                Offspring.Add(new Rabbit
                {
                    TagNumber = $"Baby-{i + 1}-{Guid.NewGuid().ToString().Substring(0, 5)}",
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
