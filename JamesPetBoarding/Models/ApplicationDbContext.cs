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

        public DbSet<VeterinarianModel> Veterinarians { get; set; }

        public DbSet<DietModel> Diets { get; set; }

        public DbSet<MedicationModel> Medications { get; set; }

        public DbSet<CustomerPetModel> CustomerPets { get; set; }

        public DbSet<PetVaccineModel> PetVaccines { get; set; }

        public DbSet<ServiceModel> Services { get; set; }

        public DbSet<InvoiceItemModel> InvoiceItems { get; set; }

        public DbSet<PaymentModel> Payments { get; set; }

        public DbSet<VaccineModel> Vaccines { get; set; }

        public DbSet<BoardingModel> Boardings { get; set; }

        public DbSet<InvoiceModel> Invoices { get; set; }

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<EmergencyContactModel> EmergencyContacts { get; set; }

        public DbSet<EmployeeModel> Employees { get; set; }

        public DbSet<OurTeamMemberModel> OurTeamMembers { get; set; }

        public DbSet<BoardingUnitModel> BoardingUnits { get; set; }

        public DbSet<ContactUsSubmissionModel> ContactUsSubmissions { get; set; }

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