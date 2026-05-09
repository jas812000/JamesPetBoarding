using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class VeterinarianModel
    {

        [Key]
        public Guid VetId { get; set; }
       
        public VeterinarianModel()
        {
            VetId = Guid.NewGuid();
        }

        [Required, MaxLength(100)]
        public string ClinicName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string Credentials { get; set; }

        [Required, MaxLength(300)]
        public string Address { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(50)]
        public string State {  get; set; }

        [Required, MaxLength(20)]
        public string ZipCode { get; set; }

        [Required, MaxLength(20)]
        public string Phone {  get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<PetModel> Pets { get; set; }

    }
}