using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataModel.Models
{
    public class PatientInfo
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(450)]
        public string UserId {  get; set; }


        [Required]
        [MaxLength(100)]
        public string treatmentPlan { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public string BirthDate { get; set; }


        [Required]
        public string treatmentDate { get; set; }


        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(200)]
        public string Hospital { get; set; }


        [Required]
        [MaxLength(200)]
        public string DoctorName {  get; set; }   
    }
}
