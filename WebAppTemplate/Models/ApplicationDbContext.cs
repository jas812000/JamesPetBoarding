using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using JamesPetBoarding.Models;

namespace JamesPetBoarding.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<PetModel> Pets { get; set; }

        public DbSet<VeterinarianModel> Veterinarian { get; set; }

        public DbSet<DietModel> Diet { get; set; }

        public DbSet<MedicationModel> Medication { get; set; }

        public DbSet<CustomerPetModel> CustomerPet { get; set; }

        public DbSet<PetVaccineModel> PetVaccine { get; set; }

        public DbSet<ServiceModel> Service { get; set; }

        public DbSet<InvoiceItemModel> InvoiceItem { get; set; }

        public DbSet<PaymentModel> Payment { get; set; }

        public DbSet<VaccineModel> Vaccine { get; set; }

        public DbSet<BoardingModel> Boarding { get; set; }

        public DbSet<InvoiceModel> Invoice { get; set; }

        public DbSet<CustomerModel> Customer { get; set; }

        public DbSet<EmergencyContactModel> EmergencyContact { get; set; }

        public DbSet<EmployeeModel> Employee { get; set; }

        public DbSet<BoardingUnitModel> BoardingUnit { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }
}