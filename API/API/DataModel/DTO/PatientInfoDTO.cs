using System.ComponentModel.DataAnnotations;

namespace API.DataModel.DTO
{
    public class PatientInfoDTO
    {
      

        [Required]
        public string email { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(200)]
        public string Hospital { get; set; }
    }
}
