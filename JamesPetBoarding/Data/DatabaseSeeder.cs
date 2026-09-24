using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Linq;

namespace JamesPetBoarding.Data
{
    public static partial class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            SeedEmployees(context);
            SeedOurTeamMembers(context);
            SeedVeterinarians(context);
            SeedVaccines(context);
            SeedServices(context);
            SeedBoardingUnits(context);
            SeedCustomersAndPets(context);
            SeedTransactions(context);

            context.SaveChanges();
        }

        private static void SeedEmployees(ApplicationDbContext context)
        {
            SeedEmployee(
                context,
                "CEBD6D3B-8E98-464D-993F-31B58D08FC9A",
                "System",
                "Administrator",
                EmployeeRoleEnum.Admin,
                "555-555-0100",
                "james.sandbox.lab@gmail.com",
                null,
                "Initial development administrator.");

            SeedEmployee(context, "1379090F-F301-4EF4-B3DE-75B118DCA947", "James", "Stevens", EmployeeRoleEnum.Admin, "972-555-0101", "james.stevens@pawsreservations.com.test", "~/Content/Images/Employees/employee-1379090ff3014ef4b3de75b118dca947.png", null);
            SeedEmployee(context, "AE5680DC-09E8-4ACA-B55F-D04E555C426E", "Celeste", "Stevens", EmployeeRoleEnum.Admin, "972-555-0102", "celeste.stevens@pawsreservations.com.test", "~/Content/Images/Employees/employee-ae5680dc09e84acab55fd04e555c426e.png", null);
            SeedEmployee(context, "68024D9B-EE2B-4E13-AF6E-A450181D0E1A", "John", "Stevens", EmployeeRoleEnum.Manager, "972-555-0103", "john.stevens@pawsreservations.com.test", "~/Content/Images/Employees/employee-68024d9bee2b4e13af6ea450181d0e1a.png", null);
            SeedEmployee(context, "9DF4B36C-DFE7-4846-93D0-1938ECD9D51B", "Elsa", "Marte", EmployeeRoleEnum.Manager, "972-555-0109", "elsa.marte@pawsreservations.com.test", "~/Content/Images/Employees/employee-9df4b36cdfe7484693d01938ecd9d51b.png", null);
            SeedEmployee(context, "EF72F732-B3EB-4993-851A-307FE8F8EF64", "Lilly", "Stevens", EmployeeRoleEnum.Manager, "972-555-0104", "lilly.stevens@pawsreservations.com.test", "~/Content/Images/Employees/employee-ef72f732b3eb4993851a307fe8f8ef64.png", null);
            SeedEmployee(context, "22624912-812B-42C1-9917-2408EA4F1CD7", "Ramon", "Marte", EmployeeRoleEnum.Manager, "972-555-0100", "ramon.marte@pawsreservations.com.test", "~/Content/Images/Employees/employee-22624912812b42c199172408ea4f1cd7.png", null);
            SeedEmployee(context, "7A6FDA45-B4E7-44BB-9911-183A59B3E1B4", "Andrew", "Lara", EmployeeRoleEnum.Supervisor, "972-555-0106", "andrew.lara@pawsreservations.com.test", null, null);
            SeedEmployee(context, "6B2A5247-8253-4669-9E46-920F275B8343", "Alex", "Lara", EmployeeRoleEnum.Supervisor, "972-555-0113", "alex.lara@pawsreservations.com.test", null, null);

            SeedEmployee(context, "10000000-0000-0000-0000-000000000001", "Victoria", "Samuel", EmployeeRoleEnum.FrontDesk, "972-555-0201", "victoria.samuel@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000002", "Nia", "Rosario", EmployeeRoleEnum.FrontDesk, "972-555-0202", "nia.rosario@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000003", "Julian", "Torres", EmployeeRoleEnum.FrontDesk, "972-555-0203", "julian.torres@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000004", "Bianca", "Castillo", EmployeeRoleEnum.FrontDesk, "972-555-0204", "bianca.castillo@pawsreservations.com.test", null, null);

            SeedEmployee(context, "10000000-0000-0000-0000-000000000005", "Basilio", "Samuel", EmployeeRoleEnum.KennelStaff, "972-555-0205", "basilio.samuel@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000006", "Andre", "Rosario", EmployeeRoleEnum.KennelStaff, "972-555-0206", "andre.rosario@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000007", "Naomi", "Cruz", EmployeeRoleEnum.KennelStaff, "972-555-0207", "naomi.cruz@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000008", "Miguel", "Reyes", EmployeeRoleEnum.KennelStaff, "972-555-0208", "miguel.reyes@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000009", "Yadira", "Mosquera", EmployeeRoleEnum.KennelStaff, "972-555-0209", "yadira.mosquera@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000010", "Joel", "Perea", EmployeeRoleEnum.KennelStaff, "972-555-0210", "joel.perea@pawsreservations.com.test", null, null);

            SeedEmployee(context, "10000000-0000-0000-0000-000000000011", "Caridad", "Samuel", EmployeeRoleEnum.Caretaker, "972-555-0211", "caridad.samuel@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000012", "Amara", "Quiñones", EmployeeRoleEnum.Caretaker, "972-555-0212", "amara.quinones@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000013", "Xavier", "Palacios", EmployeeRoleEnum.Caretaker, "972-555-0213", "xavier.palacios@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000014", "Noemí", "Córdoba", EmployeeRoleEnum.Caretaker, "972-555-0214", "noemi.cordoba@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000015", "César", "Mina", EmployeeRoleEnum.Caretaker, "972-555-0215", "cesar.mina@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000016", "Altagracia", "Peña", EmployeeRoleEnum.Caretaker, "972-555-0216", "altagracia.pena@pawsreservations.com.test", null, null);

            SeedEmployee(context, "59ADD197-995F-4FD1-9077-EA9D40FCB984", "Aaron", "Lara", EmployeeRoleEnum.Groomer, "972-555-0107", "aaron.lara@pawsreservations.com.test", "~/Content/Images/Employees/employee-59add197995f4fd19077ea9d40fcb984.png", null);
            SeedEmployee(context, "DB78ABEE-5E5E-44AF-A187-3AF3F12BBE9C", "Angelina", "Zuniga", EmployeeRoleEnum.Groomer, "972-555-0108", "angelina.zuniga@pawsreservations.com.test", "~/Content/Images/Employees/employee-db78abee5e5e44afa1873af3f12bbe9c.png", null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000017", "Imani", "Delgado", EmployeeRoleEnum.Groomer, "972-555-0217", "imani.delgado@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000018", "Rafael", "Caicedo", EmployeeRoleEnum.Groomer, "972-555-0218", "rafael.caicedo@pawsreservations.com.test", null, null);

            SeedEmployee(context, "10000000-0000-0000-0000-000000000019", "Zuri", "Herrera", EmployeeRoleEnum.VeterinaryTechnician, "972-555-0219", "zuri.herrera@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000020", "Nicolás", "Murillo", EmployeeRoleEnum.VeterinaryTechnician, "972-555-0220", "nicolas.murillo@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000021", "Tatiana", "Cuero", EmployeeRoleEnum.VeterinaryTechnician, "972-555-0221", "tatiana.cuero@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000022", "Malik", "Santiago", EmployeeRoleEnum.VeterinaryTechnician, "972-555-0222", "malik.santiago@pawsreservations.com.test", null, null);

            SeedEmployee(context, "1DEDD674-4BFC-48C2-AA85-97DD251201B6", "Ana", "Lara", EmployeeRoleEnum.Veterinarian, "972-555-0105", "ana.lara@pawsreservations.com.test", "~/Content/Images/Employees/employee-1dedd6744bfc48c2aa8597dd251201b6.png", null);
            SeedEmployee(context, "E9855D1F-FDAD-42FD-A23F-452A36705088", "Job", "Lara", EmployeeRoleEnum.Veterinarian, "972-555-0112", "job.lara@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000023", "Camila", "Navarro", EmployeeRoleEnum.Veterinarian, "972-555-0223", "camila.navarro@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000024", "Javier", "Mosquera", EmployeeRoleEnum.Veterinarian, "972-555-0224", "javier.mosquera@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000025", "Elena", "Paredes", EmployeeRoleEnum.Veterinarian, "972-555-0225", "elena.paredes@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000026", "Marcus", "de la Cruz", EmployeeRoleEnum.Veterinarian, "972-555-0226", "marcus.delacruz@pawsreservations.com.test", null, null);

            SeedEmployee(context, "10000000-0000-0000-0000-000000000027", "Ayana", "Batista", EmployeeRoleEnum.Trainer, "972-555-0227", "ayana.batista@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000028", "Gabriel", "Santana", EmployeeRoleEnum.Trainer, "972-555-0228", "gabriel.santana@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000029", "Nayeli", "Valera", EmployeeRoleEnum.Trainer, "972-555-0229", "nayeli.valera@pawsreservations.com.test", null, null);
            SeedEmployee(context, "10000000-0000-0000-0000-000000000030", "Dante", "Encarnación", EmployeeRoleEnum.Trainer, "972-555-0230", "dante.encarnacion@pawsreservations.com.test", null, null);

            context.SaveChanges();
        }

        private static void SeedEmployee(
            ApplicationDbContext context,
            string employeeId,
            string firstName,
            string lastName,
            EmployeeRoleEnum role,
            string phone,
            string email,
            string profileImagePath,
            string notes)
        {
            EmployeeModel employee = context.Employees
                .SingleOrDefault(x => x.Email == email);

            if (employee == null)
            {
                employee = new EmployeeModel
                {
                    EmployeeId = Guid.Parse(employeeId),
                    Email = email
                };

                context.Employees.Add(employee);
            }

            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Role = role;
            employee.Phone = phone;
            employee.Email = email;
            employee.IsActive = true;
            employee.InactivationReason = null;
            employee.InactivationDate = null;
            employee.InactivationNotes = null;
            employee.ReactivationDate = null;
            employee.ReactivationNotes = null;
            employee.Notes = notes;

            if (!string.IsNullOrWhiteSpace(profileImagePath))
            {
                employee.ProfileImagePath = profileImagePath;
            }
        }

        private static void SeedOurTeamMembers(ApplicationDbContext context)
        {
            SeedOurTeamMember(context, "775C5FB3-835D-4C5F-9D83-3BDD59BF1AD2", "james.stevens@pawsreservations.com.test", "Owner & Lead Caretaker", 1);
            SeedOurTeamMember(context, "5544E700-89A0-426C-8291-A4086E50C2B6", "celeste.stevens@pawsreservations.com.test", "Administrative Manager", 2);
            SeedOurTeamMember(context, "478E98E8-B02C-437D-9FA9-3A240C9A9429", "john.stevens@pawsreservations.com.test", "Facilities & Supplies Manager", 3);
            SeedOurTeamMember(context, "9F878845-5A36-4E8A-8907-8318A37037FF", "elsa.marte@pawsreservations.com.test", "Boarding Operations Manager", 4);
            SeedOurTeamMember(context, "D9419D21-33FA-4BBE-9B83-FAF8F760F21C", "andrew.lara@pawsreservations.com.test", "Pet Care Supervisor", 5);
            SeedOurTeamMember(context, "C855FB53-C69F-46F7-9932-0D5FC83184A3", "ana.lara@pawsreservations.com.test", "In-House Veterinarian", 6);
            SeedOurTeamMember(context, "CEE41020-A9B1-4DAE-A380-B0FC9C6AB010", "job.lara@pawsreservations.com.test", "Associate Veterinarian", 7);
            SeedOurTeamMember(context, "F48AC0E7-D79F-4B89-AA29-BB6A257230E4", "ramon.marte@pawsreservations.com.test", "Veterinary Services Manager", 8);
            SeedOurTeamMember(context, "0A1B9912-5C51-4A64-924B-CC70C25C4AE7", "lilly.stevens@pawsreservations.com.test", "Pet Care Manager", 9);
            SeedOurTeamMember(context, "B77F6719-3BC8-41D4-8F5D-919D6F9AD9E5", "alex.lara@pawsreservations.com.test", "Kennel Operations Supervisor", 10);
            SeedOurTeamMember(context, "B46E7244-0A5D-4ADE-BD6F-7F98032D2A4E", "aaron.lara@pawsreservations.com.test", "Pet Groomer", 11);
            SeedOurTeamMember(context, "92DF13DD-F9B8-40B3-8A9B-171FE42E8AA4", "angelina.zuniga@pawsreservations.com.test", "Pet Groomer", 12);

            context.SaveChanges();
        }

        private static void SeedOurTeamMember(
            ApplicationDbContext context,
            string ourTeamMemberId,
            string employeeEmail,
            string publicJobTitle,
            int displayOrder)
        {
            EmployeeModel employee = context.Employees
                .Single(x => x.Email == employeeEmail);

            OurTeamMemberModel teamMember = context.OurTeamMembers
                .SingleOrDefault(x => x.EmployeeId == employee.EmployeeId);

            if (teamMember == null)
            {
                teamMember = new OurTeamMemberModel
                {
                    OurTeamMemberId = Guid.Parse(ourTeamMemberId),
                    EmployeeId = employee.EmployeeId
                };

                context.OurTeamMembers.Add(teamMember);
            }

            teamMember.PublicJobTitle = publicJobTitle;
            teamMember.DisplayOrder = displayOrder;
        }

        private static void SeedVeterinarians(ApplicationDbContext context)
        {
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000001", "Paws & Reservations Veterinary Center", "Ana", "Lara", "DVM", "2250 Barkwood Lane", "Dallas", StateEnum.TX, "75201", "972-555-0301", "ana.lara.vet@pawsreservations.com.test", true, "In-house veterinarian.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000002", "Paws & Reservations Veterinary Center", "Job", "Lara", "DVM", "2250 Barkwood Lane", "Dallas", StateEnum.TX, "75201", "972-555-0302", "job.lara.vet@pawsreservations.com.test", true, "In-house veterinarian.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000003", "Paws & Reservations Veterinary Center", "Camila", "Navarro", "DVM", "2250 Barkwood Lane", "Dallas", StateEnum.TX, "75201", "972-555-0303", "camila.navarro.vet@pawsreservations.com.test", true, "In-house veterinarian.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000004", "Paws & Reservations Veterinary Center", "Javier", "Mosquera", "DVM", "2250 Barkwood Lane", "Dallas", StateEnum.TX, "75201", "972-555-0304", "javier.mosquera.vet@pawsreservations.com.test", true, "In-house veterinarian.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000005", "Paws & Reservations Veterinary Center", "Elena", "Paredes", "DVM", "2250 Barkwood Lane", "Dallas", StateEnum.TX, "75201", "972-555-0305", "elena.paredes.vet@pawsreservations.com.test", true, "In-house veterinarian.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000006", "Paws & Reservations Veterinary Center", "Marcus", "de la Cruz", "DVM", "2250 Barkwood Lane", "Dallas", StateEnum.TX, "75201", "972-555-0306", "marcus.delacruz.vet@pawsreservations.com.test", true, "In-house veterinarian.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000007", "Oak Valley Animal Clinic", "Maritza", "Coleman", "DVM", "1189 Oak Valley Road", "Dallas", StateEnum.TX, "75204", "972-555-0307", "maritza.coleman@oakvalley.test", true, "External veterinary partner.");
            SeedVeterinarian(context, "20000000-0000-0000-0000-000000000008", "Lakeside Pet Hospital", "Devon", "Price", "DVM", "740 Lakeview Boulevard", "Dallas", StateEnum.TX, "75214", "972-555-0308", "devon.price@lakesidepet.test", true, "Emergency veterinary partner.");

            context.SaveChanges();
        }

        private static void SeedVeterinarian(
            ApplicationDbContext context,
            string veterinarianId,
            string clinicName,
            string firstName,
            string lastName,
            string credentials,
            string address,
            string city,
            StateEnum state,
            string zipCode,
            string phone,
            string email,
            bool isActive,
            string notes)
        {
            VeterinarianModel veterinarian = context.Veterinarians
                .SingleOrDefault(x => x.Email == email);

            if (veterinarian == null)
            {
                veterinarian = new VeterinarianModel
                {
                    VetId = Guid.Parse(veterinarianId),
                    Email = email
                };

                context.Veterinarians.Add(veterinarian);
            }

            veterinarian.ClinicName = clinicName;
            veterinarian.FirstName = firstName;
            veterinarian.LastName = lastName;
            veterinarian.Credentials = credentials;
            veterinarian.Address = address;
            veterinarian.City = city;
            veterinarian.State = state;
            veterinarian.ZipCode = zipCode;
            veterinarian.Phone = phone;
            veterinarian.Email = email;
            veterinarian.IsActive = isActive;
            veterinarian.Notes = notes;
        }

        private static void SeedVaccines(ApplicationDbContext context)
        {
            SeedVaccine(context, "30000000-0000-0000-0000-000000000001", "Rabies", SpeciesEnum.Dog, true, "Required core canine vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000002", "DHPP", SpeciesEnum.Dog, true, "Canine distemper, hepatitis, parainfluenza, and parvovirus.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000003", "Bordetella", SpeciesEnum.Dog, true, "Required for boarding.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000004", "Leptospirosis", SpeciesEnum.Dog, false, "Recommended based on exposure risk.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000005", "Canine Influenza", SpeciesEnum.Dog, false, "Recommended for dogs in group settings.");

            SeedVaccine(context, "30000000-0000-0000-0000-000000000006", "Rabies", SpeciesEnum.Cat, true, "Required core feline vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000007", "FVRCP", SpeciesEnum.Cat, true, "Core feline combination vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000008", "Feline Leukemia", SpeciesEnum.Cat, false, "Recommended for cats with outdoor exposure.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000009", "Bordetella", SpeciesEnum.Cat, false, "Recommended for some boarding environments.");

            SeedVaccine(context, "30000000-0000-0000-0000-000000000010", "Polyomavirus", SpeciesEnum.Bird, true, "Required for boarded birds.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000011", "RHDV2", SpeciesEnum.Rabbit, true, "Rabbit hemorrhagic disease vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000012", "Rabies", SpeciesEnum.Horse, true, "Core equine vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000013", "Tetanus", SpeciesEnum.Horse, true, "Core equine vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000014", "West Nile Virus", SpeciesEnum.Horse, true, "Core equine vaccine.");
            SeedVaccine(context, "30000000-0000-0000-0000-000000000015", "EEE/WEE", SpeciesEnum.Horse, true, "Eastern and Western equine encephalomyelitis.");

            context.SaveChanges();
        }

        private static void SeedVaccine(
            ApplicationDbContext context,
            string vaccineId,
            string vaccineName,
            SpeciesEnum species,
            bool requiredFlag,
            string notes)
        {
            VaccineModel vaccine = context.Vaccines
                .SingleOrDefault(x =>
                    x.VaccineName == vaccineName &&
                    x.Species == species);

            if (vaccine == null)
            {
                vaccine = new VaccineModel
                {
                    VaccineId = Guid.Parse(vaccineId),
                    VaccineName = vaccineName,
                    Species = species
                };

                context.Vaccines.Add(vaccine);
            }

            vaccine.RequiredFlag = requiredFlag;
            vaccine.Notes = notes;
        }

        private static void SeedServices(ApplicationDbContext context)
        {
            SeedService(context, "40000000-0000-0000-0000-000000000001", ServiceNameEnum.FullGrooming, SpeciesEnum.Dog, 65.00m, PricingTypeEnum.PerService, "Full canine grooming service.");
            SeedService(context, "40000000-0000-0000-0000-000000000002", ServiceNameEnum.FullGrooming, SpeciesEnum.Cat, 75.00m, PricingTypeEnum.PerService, "Full feline grooming service.");
            SeedService(context, "40000000-0000-0000-0000-000000000003", ServiceNameEnum.NailTrim, SpeciesEnum.Dog, 18.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000004", ServiceNameEnum.NailTrim, SpeciesEnum.Cat, 20.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000005", ServiceNameEnum.Bath, SpeciesEnum.Dog, 35.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000006", ServiceNameEnum.Bath, SpeciesEnum.Cat, 45.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000007", ServiceNameEnum.EarCleaning, SpeciesEnum.Dog, 15.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000008", ServiceNameEnum.TeethBrushing, SpeciesEnum.Dog, 12.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000009", ServiceNameEnum.HandFeeding, SpeciesEnum.Dog, 8.00m, PricingTypeEnum.PerDay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000010", ServiceNameEnum.FoodPreparation, SpeciesEnum.Dog, 10.00m, PricingTypeEnum.PerDay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000011", ServiceNameEnum.MedicationAdministration, SpeciesEnum.Dog, 12.00m, PricingTypeEnum.PerDay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000012", ServiceNameEnum.MedicationAdministration, SpeciesEnum.Cat, 12.00m, PricingTypeEnum.PerDay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000013", ServiceNameEnum.ExtraPlayTime, SpeciesEnum.Dog, 20.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000014", ServiceNameEnum.ExtraWalk, SpeciesEnum.Dog, 18.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000015", ServiceNameEnum.OneOnOnePlay, SpeciesEnum.Cat, 20.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000016", ServiceNameEnum.LatePickUp, SpeciesEnum.Dog, 30.00m, PricingTypeEnum.PerStay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000017", ServiceNameEnum.AfterHoursPickup, SpeciesEnum.Dog, 45.00m, PricingTypeEnum.PerStay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000018", ServiceNameEnum.EarlyDropOff, SpeciesEnum.Dog, 25.00m, PricingTypeEnum.PerStay, null);
            SeedService(context, "40000000-0000-0000-0000-000000000019", ServiceNameEnum.BoardingUpgrade, SpeciesEnum.Dog, 20.00m, PricingTypeEnum.PerNight, null);
            SeedService(context, "40000000-0000-0000-0000-000000000020", ServiceNameEnum.LuxurySuiteUpgrade, SpeciesEnum.Dog, 40.00m, PricingTypeEnum.PerNight, null);
            SeedService(context, "40000000-0000-0000-0000-000000000021", ServiceNameEnum.TrainingSession, SpeciesEnum.Dog, 55.00m, PricingTypeEnum.PerHour, null);
            SeedService(context, "40000000-0000-0000-0000-000000000022", ServiceNameEnum.BehavioralAssessment, SpeciesEnum.Dog, 85.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000023", ServiceNameEnum.Detangling, SpeciesEnum.Dog, 25.00m, PricingTypeEnum.PerService, null);
            SeedService(context, "40000000-0000-0000-0000-000000000024", ServiceNameEnum.Detangling, SpeciesEnum.Cat, 30.00m, PricingTypeEnum.PerService, null);

            context.SaveChanges();
        }

        private static void SeedService(
            ApplicationDbContext context,
            string serviceId,
            ServiceNameEnum serviceName,
            SpeciesEnum species,
            decimal basePrice,
            PricingTypeEnum pricingType,
            string notes)
        {
            ServiceModel service = context.Services
                .SingleOrDefault(x =>
                    x.ServiceName == serviceName &&
                    x.Species == species);

            if (service == null)
            {
                service = new ServiceModel
                {
                    ServiceId = Guid.Parse(serviceId),
                    ServiceName = serviceName,
                    Species = species
                };

                context.Services.Add(service);
            }

            service.BasePrice = basePrice;
            service.PricingType = pricingType;
            service.Notes = notes;
        }

        private static void SeedBoardingUnits(ApplicationDbContext context)
        {
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000001", UnitTypeEnum.Kennel, UnitNameEnum.Mercury, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.Small, true, "Small-dog kennel.");
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000002", UnitTypeEnum.Kennel, UnitNameEnum.Venus, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.Medium, true, "Medium-dog kennel.");
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000003", UnitTypeEnum.Kennel, UnitNameEnum.Earth, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.Large, true, "Large-dog kennel.");
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000004", UnitTypeEnum.Kennel, UnitNameEnum.Mars, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.ExtraLarge, true, "Extra-large-dog kennel.");
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000005", UnitTypeEnum.Kennel, UnitNameEnum.Jupiter, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.AnySize, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000006", UnitTypeEnum.Kennel, UnitNameEnum.Saturn, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.AnySize, true, null);

            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000007", UnitTypeEnum.Suite, UnitNameEnum.Diamond, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.Large, true, "Luxury canine suite.");
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000008", UnitTypeEnum.Suite, UnitNameEnum.Jade, 1, SpeciesAllowedEnum.Canine, SizeCategoryEnum.AnySize, true, "Premium canine suite.");
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000009", UnitTypeEnum.Suite, UnitNameEnum.Pearl, 1, SpeciesAllowedEnum.Mixed, SizeCategoryEnum.AnySize, true, "Flexible premium suite.");

            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000010", UnitTypeEnum.CatCondo, UnitNameEnum.Paris, 1, SpeciesAllowedEnum.Feline, SizeCategoryEnum.Small, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000011", UnitTypeEnum.CatCondo, UnitNameEnum.Rome, 1, SpeciesAllowedEnum.Feline, SizeCategoryEnum.Small, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000012", UnitTypeEnum.CatCondo, UnitNameEnum.Tokyo, 1, SpeciesAllowedEnum.Feline, SizeCategoryEnum.Small, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000013", UnitTypeEnum.CatCondo, UnitNameEnum.Vienna, 1, SpeciesAllowedEnum.Feline, SizeCategoryEnum.Small, true, null);

            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000014", UnitTypeEnum.BirdCage, UnitNameEnum.Oak, 1, SpeciesAllowedEnum.Avian, SizeCategoryEnum.Small, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000015", UnitTypeEnum.BirdCage, UnitNameEnum.Maple, 1, SpeciesAllowedEnum.Avian, SizeCategoryEnum.Medium, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000016", UnitTypeEnum.SmallAnimalEnclosure, UnitNameEnum.Carnation, 1, SpeciesAllowedEnum.SmallMammal, SizeCategoryEnum.Small, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000017", UnitTypeEnum.SmallAnimalEnclosure, UnitNameEnum.Rose, 1, SpeciesAllowedEnum.Reptile, SizeCategoryEnum.Small, true, null);
            SeedBoardingUnit(context, "50000000-0000-0000-0000-000000000018", UnitTypeEnum.Isolation, UnitNameEnum.Red, 1, SpeciesAllowedEnum.Mixed, SizeCategoryEnum.AnySize, true, "Isolation unit for veterinary or health-related stays.");

            context.SaveChanges();
        }

        private static void SeedBoardingUnit(
            ApplicationDbContext context,
            string boardingUnitId,
            UnitTypeEnum unitType,
            UnitNameEnum unitName,
            int unitNumber,
            SpeciesAllowedEnum speciesAllowed,
            SizeCategoryEnum sizeCategory,
            bool isActive,
            string notes)
        {
            BoardingUnitModel boardingUnit = context.BoardingUnits
                .SingleOrDefault(x =>
                    x.UnitName == unitName &&
                    x.UnitNumber == unitNumber);

            if (boardingUnit == null)
            {
                boardingUnit = new BoardingUnitModel
                {
                    BoardingUnitId = Guid.Parse(boardingUnitId),
                    UnitName = unitName,
                    UnitNumber = unitNumber
                };

                context.BoardingUnits.Add(boardingUnit);
            }

            boardingUnit.UnitType = unitType;
            boardingUnit.SpeciesAllowed = speciesAllowed;
            boardingUnit.SizeCategory = sizeCategory;
            boardingUnit.IsActive = isActive;
            boardingUnit.Notes = notes;
        }
    }
}
