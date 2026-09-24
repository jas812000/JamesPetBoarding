using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace JamesPetBoarding.Data
{
    public static partial class DatabaseSeeder
    {
        private static void SeedCustomersAndPets(
            ApplicationDbContext context)
        {
            SeedCustomers(context);
            SeedEmergencyContacts(context);
            SeedPets(context);
            SeedCustomerPetRelationships(context);
            SeedDiets(context);
            SeedMedications(context);
            SeedPetVaccines(context);

            context.SaveChanges();
        }

        private static void SeedCustomers(
            ApplicationDbContext context)
        {
            List<CustomerModel> customers =
                new List<CustomerModel>
                {
                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000001",
                        "Danielle",
                        "Brooks",
                        "4102 Cedar Springs Road",
                        "Dallas",
                        "75219",
                        "214-555-0201",
                        "danielle.brooks@example.test",
                        "Prefers text-message appointment reminders."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000002",
                        "Marcus",
                        "Johnson",
                        "1820 Oak Lawn Avenue",
                        "Dallas",
                        "75207",
                        "214-555-0202",
                        "marcus.johnson@example.test",
                        "Frequent boarding customer."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000003",
                        "Sofía",
                        "Mendoza",
                        "7315 Gaston Avenue",
                        "Dallas",
                        "75214",
                        "214-555-0203",
                        "sofia.mendoza@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000004",
                        "Terrence",
                        "Williams",
                        "905 North Bishop Avenue",
                        "Dallas",
                        "75208",
                        "214-555-0204",
                        "terrence.williams@example.test",
                        "Authorized pickups must present identification."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000005",
                        "Marisol",
                        "Rivera",
                        "5621 Swiss Avenue",
                        "Dallas",
                        "75214",
                        "214-555-0205",
                        "marisol.rivera@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000006",
                        "Alicia",
                        "Thompson",
                        "2840 Greenville Avenue",
                        "Dallas",
                        "75206",
                        "214-555-0206",
                        "alicia.thompson@example.test",
                        "Call before administering any as-needed medication."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000007",
                        "Javier",
                        "Cruz",
                        "3207 West Davis Street",
                        "Dallas",
                        "75211",
                        "214-555-0207",
                        "javier.cruz@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000008",
                        "Monique",
                        "Baptiste",
                        "4710 Ross Avenue",
                        "Dallas",
                        "75204",
                        "214-555-0208",
                        "monique.baptiste@example.test",
                        "Has two pets that normally board together."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000009",
                        "Gabriela",
                        "Santos",
                        "8130 Meadow Road",
                        "Dallas",
                        "75231",
                        "214-555-0209",
                        "gabriela.santos@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000010",
                        "Darius",
                        "Coleman",
                        "1560 South Ervay Street",
                        "Dallas",
                        "75215",
                        "214-555-0210",
                        "darius.coleman@example.test",
                        "New customer referred by Oak Valley Animal Clinic."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000011",
                        "Valentina",
                        "Paredes",
                        "6305 La Vista Drive",
                        "Dallas",
                        "75214",
                        "214-555-0211",
                        "valentina.paredes@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000012",
                        "Isaiah",
                        "Robinson",
                        "2209 Elm Street",
                        "Dallas",
                        "75201",
                        "214-555-0212",
                        "isaiah.robinson@example.test",
                        "Requires early drop-off for weekday boardings."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000013",
                        "Camila",
                        "Navarro",
                        "7720 Campbell Road",
                        "Dallas",
                        "75248",
                        "214-555-0213",
                        "camila.navarro.customer@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000014",
                        "Andre",
                        "Whitfield",
                        "3901 Maple Avenue",
                        "Dallas",
                        "75219",
                        "214-555-0214",
                        "andre.whitfield@example.test",
                        null),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000015",
                        "Natalia",
                        "Mosquera",
                        "1447 North Zang Boulevard",
                        "Dallas",
                        "75203",
                        "214-555-0215",
                        "natalia.mosquera@example.test",
                        "Pet has detailed feeding instructions."),

                    CreateCustomer(
                        "60000000-0000-0000-0000-000000000016",
                        "Elijah",
                        "Price",
                        "9850 Walnut Hill Lane",
                        "Dallas",
                        "75238",
                        "214-555-0216",
                        "elijah.price@example.test",
                        null)
                };

            foreach (CustomerModel customer in customers)
            {
                CustomerModel existingCustomer =
                    context.Customers.FirstOrDefault(
                        x => x.Email == customer.Email);

                if (existingCustomer == null)
                {
                    context.Customers.Add(customer);
                }
                else
                {
                    existingCustomer.FirstName = customer.FirstName;
                    existingCustomer.LastName = customer.LastName;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.City = customer.City;
                    existingCustomer.State = customer.State;
                    existingCustomer.ZipCode = customer.ZipCode;
                    existingCustomer.Phone = customer.Phone;
                    existingCustomer.IsActive = customer.IsActive;
                    existingCustomer.Notes = customer.Notes;
                }
            }

            context.SaveChanges();
        }

        private static CustomerModel CreateCustomer(
            string customerId,
            string firstName,
            string lastName,
            string address,
            string city,
            string zipCode,
            string phone,
            string email,
            string notes)
        {
            return new CustomerModel
            {
                CustomerId = Guid.Parse(customerId),
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                City = city,
                State = StateEnum.TX,
                ZipCode = zipCode,
                Phone = phone,
                Email = email,
                IsActive = true,
                Notes = notes
            };
        }

        private static void SeedEmergencyContacts(
            ApplicationDbContext context)
        {
            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000001",
                "danielle.brooks@example.test",
                "Avery",
                "Brooks",
                "4102 Cedar Springs Road",
                "214-555-0301",
                "avery.brooks@example.test",
                EmergencyContactRelationshipEnum.Spouse);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000002",
                "marcus.johnson@example.test",
                "Denise",
                "Johnson",
                "1820 Oak Lawn Avenue",
                "214-555-0302",
                "denise.johnson@example.test",
                EmergencyContactRelationshipEnum.Sister);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000003",
                "sofia.mendoza@example.test",
                "Luis",
                "Mendoza",
                "7315 Gaston Avenue",
                "214-555-0303",
                "luis.mendoza@example.test",
                EmergencyContactRelationshipEnum.Brother);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000004",
                "terrence.williams@example.test",
                "Kiara",
                "Williams",
                "905 North Bishop Avenue",
                "214-555-0304",
                "kiara.williams@example.test",
                EmergencyContactRelationshipEnum.Spouse);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000005",
                "marisol.rivera@example.test",
                "Elena",
                "Rivera",
                "5621 Swiss Avenue",
                "214-555-0305",
                "elena.rivera@example.test",
                EmergencyContactRelationshipEnum.Mother);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000006",
                "alicia.thompson@example.test",
                "Jordan",
                "Miles",
                "2900 Greenville Avenue",
                "214-555-0306",
                "jordan.miles@example.test",
                EmergencyContactRelationshipEnum.Friend);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000007",
                "javier.cruz@example.test",
                "Rosa",
                "Cruz",
                "3207 West Davis Street",
                "214-555-0307",
                "rosa.cruz@example.test",
                EmergencyContactRelationshipEnum.Mother);

            AddOrUpdateEmergencyContact(
                context,
                "61000000-0000-0000-0000-000000000008",
                "monique.baptiste@example.test",
                "Claude",
                "Baptiste",
                "4710 Ross Avenue",
                "214-555-0308",
                "claude.baptiste@example.test",
                EmergencyContactRelationshipEnum.Father);

            context.SaveChanges();
        }

        private static void AddOrUpdateEmergencyContact(
            ApplicationDbContext context,
            string emergencyContactId,
            string customerEmail,
            string firstName,
            string lastName,
            string address,
            string phone,
            string email,
            EmergencyContactRelationshipEnum relationship)
        {
            CustomerModel customer =
                GetCustomer(context, customerEmail);

            Guid id = Guid.Parse(emergencyContactId);

            EmergencyContactModel emergencyContact =
                context.EmergencyContacts.FirstOrDefault(
                    x => x.EmergencyContactId == id);

            if (emergencyContact == null)
            {
                emergencyContact = new EmergencyContactModel
                {
                    EmergencyContactId = id
                };

                context.EmergencyContacts.Add(emergencyContact);
            }

            emergencyContact.CustomerId = customer.CustomerId;
            emergencyContact.FirstName = firstName;
            emergencyContact.LastName = lastName;
            emergencyContact.Address = address;
            emergencyContact.City = "Dallas";
            emergencyContact.State = StateEnum.TX;
            emergencyContact.ZipCode = "75219";
            emergencyContact.Phone = phone;
            emergencyContact.Email = email;
            emergencyContact.RelationshipType = relationship;
            emergencyContact.IsActive = true;
        }

        private static void SeedPets(
            ApplicationDbContext context)
        {
            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000001",
                "danielle.brooks@example.test", "Bella", SpeciesEnum.Dog,
                "Golden Retriever", SexEnum.Female, 6, 63.50m,
                "ana.lara@pawsreservations.com.test",
                "Friendly; mild separation anxiety during the first night.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000002",
                "marcus.johnson@example.test", "Duke", SpeciesEnum.Dog,
                "German Shepherd", SexEnum.Male, 5, 81.25m,
                "job.lara@pawsreservations.com.test",
                "Responds to basic obedience commands.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000003",
                "sofia.mendoza@example.test", "Luna", SpeciesEnum.Cat,
                "Domestic Shorthair", SexEnum.Female, 4, 10.80m,
                "maritza.coleman@oakvalley.example.test",
                "Prefers quiet housing away from dogs.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000004",
                "terrence.williams@example.test", "Rocky", SpeciesEnum.Dog,
                "Boxer", SexEnum.Male, 7, 68.40m,
                "ana.lara@pawsreservations.com.test",
                "No group play without staff supervision.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000005",
                "marisol.rivera@example.test", "Coco", SpeciesEnum.Bird,
                "African Grey Parrot", SexEnum.Female, 9, 1.10m,
                "devon.price@lakeside.example.test",
                "Keep cage covered between 8 PM and 7 AM.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000006",
                "alicia.thompson@example.test", "Milo", SpeciesEnum.Dog,
                "Beagle", SexEnum.Male, 8, 31.75m,
                "job.lara@pawsreservations.com.test",
                "Takes daily thyroid medication.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000007",
                "javier.cruz@example.test", "Nala", SpeciesEnum.Cat,
                "Maine Coon", SexEnum.Female, 3, 14.20m,
                "maritza.coleman@oakvalley.example.test",
                null);

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000008",
                "monique.baptiste@example.test", "Chance", SpeciesEnum.Dog,
                "Labrador Retriever", SexEnum.Male, 4, 72.30m,
                "ana.lara@pawsreservations.com.test",
                "Boards with Hazel when possible.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000009",
                "monique.baptiste@example.test", "Hazel", SpeciesEnum.Dog,
                "Labrador Retriever", SexEnum.Female, 4, 65.90m,
                "ana.lara@pawsreservations.com.test",
                "Boards with Chance when possible.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000010",
                "gabriela.santos@example.test", "Pepper", SpeciesEnum.Rabbit,
                "Mini Rex", SexEnum.Female, 2, 4.60m,
                "devon.price@lakeside.example.test",
                "Provide unlimited timothy hay.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000011",
                "darius.coleman@example.test", "Ace", SpeciesEnum.Dog,
                "American Staffordshire Terrier", SexEnum.Male, 6, 59.80m,
                "maritza.coleman@oakvalley.example.test",
                "Slow introductions to unfamiliar dogs.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000012",
                "valentina.paredes@example.test", "Simba", SpeciesEnum.Cat,
                "Siamese", SexEnum.Male, 5, 11.40m,
                "job.lara@pawsreservations.com.test",
                "Uses prescription urinary diet.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000013",
                "isaiah.robinson@example.test", "Zeus", SpeciesEnum.Dog,
                "Great Dane", SexEnum.Male, 4, 142.00m,
                "ana.lara@pawsreservations.com.test",
                "Requires an extra-large boarding unit.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000014",
                "camila.navarro.customer@example.test", "Kiwi", SpeciesEnum.Bird,
                "Green-Cheeked Conure", SexEnum.Unknown, 3, 0.18m,
                "devon.price@lakeside.example.test",
                "Enjoys supervised interaction and quiet music.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000015",
                "andre.whitfield@example.test", "Blue", SpeciesEnum.Dog,
                "Australian Cattle Dog", SexEnum.Male, 3, 47.60m,
                "job.lara@pawsreservations.com.test",
                "High energy; schedule extra exercise.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000016",
                "natalia.mosquera@example.test", "Daisy", SpeciesEnum.Dog,
                "French Bulldog", SexEnum.Female, 6, 24.50m,
                "ana.lara@pawsreservations.com.test",
                "Monitor carefully during hot weather.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000017",
                "elijah.price@example.test", "Onyx", SpeciesEnum.Reptile,
                "Bearded Dragon", SexEnum.Male, 4, 1.25m,
                "devon.price@lakeside.example.test",
                "Maintain enclosure temperature according to care sheet.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000018",
                "danielle.brooks@example.test", "Max", SpeciesEnum.Dog,
                "Miniature Schnauzer", SexEnum.Male, 10, 19.80m,
                "ana.lara@pawsreservations.com.test",
                "Senior pet; administer eye medication.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000019",
                "marcus.johnson@example.test", "Shadow", SpeciesEnum.Cat,
                "Domestic Longhair", SexEnum.Male, 7, 13.10m,
                "job.lara@pawsreservations.com.test",
                null);

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000020",
                "sofia.mendoza@example.test", "Rosie", SpeciesEnum.Dog,
                "Cocker Spaniel", SexEnum.Female, 9, 27.20m,
                "maritza.coleman@oakvalley.example.test",
                "Requires ear medication during flare-ups.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000021",
                "javier.cruz@example.test", "Apollo", SpeciesEnum.Dog,
                "Siberian Husky", SexEnum.Male, 5, 58.70m,
                "ana.lara@pawsreservations.com.test",
                "Secure all gates before moving between areas.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000022",
                "gabriela.santos@example.test", "Mochi", SpeciesEnum.Cat,
                "Ragdoll", SexEnum.Female, 2, 9.90m,
                "job.lara@pawsreservations.com.test",
                null);

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000023",
                "valentina.paredes@example.test", "Pinto", SpeciesEnum.Horse,
                "American Paint Horse", SexEnum.Male, 11, 1050.00m,
                "devon.price@lakeside.example.test",
                "Off-site equine boarding test record.");

            AddOrUpdatePet(context, "70000000-0000-0000-0000-000000000024",
                "andre.whitfield@example.test", "Thumper", SpeciesEnum.Rabbit,
                "Holland Lop", SexEnum.Male, 3, 3.80m,
                "maritza.coleman@oakvalley.example.test",
                "Provide fresh greens once daily.");

            context.SaveChanges();
        }

        private static void AddOrUpdatePet(
            ApplicationDbContext context,
            string petId,
            string customerEmail,
            string petName,
            SpeciesEnum species,
            string breed,
            SexEnum sex,
            int age,
            decimal weight,
            string veterinarianEmail,
            string notes)
        {
            Guid id = Guid.Parse(petId);

            PetModel pet =
                context.Pets.FirstOrDefault(x => x.PetId == id);

            if (pet == null)
            {
                pet = new PetModel
                {
                    PetId = id
                };

                context.Pets.Add(pet);
            }

            VeterinarianModel veterinarian =
                context.Veterinarians.FirstOrDefault(
                    x => x.Email == veterinarianEmail);

            pet.VetId = veterinarian == null
                ? (Guid?)null
                : veterinarian.VetId;

            pet.PetName = petName;
            pet.Species = species;
            pet.Breed = breed;
            pet.Sex = sex;
            pet.BirthDate = DateTime.Today
                .AddYears(-age)
                .AddDays(-30);
            pet.Weight = weight;
            pet.IsActive = true;
            pet.Notes = notes;
        }

        private static void SeedCustomerPetRelationships(
            ApplicationDbContext context)
        {
            AddOwner(context, "62000000-0000-0000-0000-000000000001",
                "danielle.brooks@example.test",
                "70000000-0000-0000-0000-000000000001");

            AddOwner(context, "62000000-0000-0000-0000-000000000002",
                "marcus.johnson@example.test",
                "70000000-0000-0000-0000-000000000002");

            AddOwner(context, "62000000-0000-0000-0000-000000000003",
                "sofia.mendoza@example.test",
                "70000000-0000-0000-0000-000000000003");

            AddOwner(context, "62000000-0000-0000-0000-000000000004",
                "terrence.williams@example.test",
                "70000000-0000-0000-0000-000000000004");

            AddOwner(context, "62000000-0000-0000-0000-000000000005",
                "marisol.rivera@example.test",
                "70000000-0000-0000-0000-000000000005");

            AddOwner(context, "62000000-0000-0000-0000-000000000006",
                "alicia.thompson@example.test",
                "70000000-0000-0000-0000-000000000006");

            AddOwner(context, "62000000-0000-0000-0000-000000000007",
                "javier.cruz@example.test",
                "70000000-0000-0000-0000-000000000007");

            AddOwner(context, "62000000-0000-0000-0000-000000000008",
                "monique.baptiste@example.test",
                "70000000-0000-0000-0000-000000000008");

            AddOwner(context, "62000000-0000-0000-0000-000000000009",
                "monique.baptiste@example.test",
                "70000000-0000-0000-0000-000000000009");

            AddOwner(context, "62000000-0000-0000-0000-000000000010",
                "gabriela.santos@example.test",
                "70000000-0000-0000-0000-000000000010");

            AddOwner(context, "62000000-0000-0000-0000-000000000011",
                "darius.coleman@example.test",
                "70000000-0000-0000-0000-000000000011");

            AddOwner(context, "62000000-0000-0000-0000-000000000012",
                "valentina.paredes@example.test",
                "70000000-0000-0000-0000-000000000012");

            AddOwner(context, "62000000-0000-0000-0000-000000000013",
                "isaiah.robinson@example.test",
                "70000000-0000-0000-0000-000000000013");

            AddOwner(context, "62000000-0000-0000-0000-000000000014",
                "camila.navarro.customer@example.test",
                "70000000-0000-0000-0000-000000000014");

            AddOwner(context, "62000000-0000-0000-0000-000000000015",
                "andre.whitfield@example.test",
                "70000000-0000-0000-0000-000000000015");

            AddOwner(context, "62000000-0000-0000-0000-000000000016",
                "natalia.mosquera@example.test",
                "70000000-0000-0000-0000-000000000016");

            AddOwner(context, "62000000-0000-0000-0000-000000000017",
                "elijah.price@example.test",
                "70000000-0000-0000-0000-000000000017");

            AddOwner(context, "62000000-0000-0000-0000-000000000018",
                "danielle.brooks@example.test",
                "70000000-0000-0000-0000-000000000018");

            AddOwner(context, "62000000-0000-0000-0000-000000000019",
                "marcus.johnson@example.test",
                "70000000-0000-0000-0000-000000000019");

            AddOwner(context, "62000000-0000-0000-0000-000000000020",
                "sofia.mendoza@example.test",
                "70000000-0000-0000-0000-000000000020");

            AddOwner(context, "62000000-0000-0000-0000-000000000021",
                "javier.cruz@example.test",
                "70000000-0000-0000-0000-000000000021");

            AddOwner(context, "62000000-0000-0000-0000-000000000022",
                "gabriela.santos@example.test",
                "70000000-0000-0000-0000-000000000022");

            AddOwner(context, "62000000-0000-0000-0000-000000000023",
                "valentina.paredes@example.test",
                "70000000-0000-0000-0000-000000000023");

            AddOwner(context, "62000000-0000-0000-0000-000000000024",
                "andre.whitfield@example.test",
                "70000000-0000-0000-0000-000000000024");

            AddRelationship(
                context,
                "62000000-0000-0000-0000-000000000025",
                "marisol.rivera@example.test",
                "70000000-0000-0000-0000-000000000003",
                RelationshipTypeEnum.AuthorizedPickup);

            AddRelationship(
                context,
                "62000000-0000-0000-0000-000000000026",
                "terrence.williams@example.test",
                "70000000-0000-0000-0000-000000000011",
                RelationshipTypeEnum.CoOwner);

            context.SaveChanges();
        }

        private static void AddOwner(
            ApplicationDbContext context,
            string customerPetId,
            string customerEmail,
            string petId)
        {
            AddRelationship(
                context,
                customerPetId,
                customerEmail,
                petId,
                RelationshipTypeEnum.Owner);
        }

        private static void AddRelationship(
            ApplicationDbContext context,
            string customerPetId,
            string customerEmail,
            string petId,
            RelationshipTypeEnum relationship)
        {
            Guid id = Guid.Parse(customerPetId);
            Guid parsedPetId = Guid.Parse(petId);

            CustomerPetModel customerPet =
                context.CustomerPets.FirstOrDefault(
                    x => x.CustomerPetId == id);

            if (customerPet == null)
            {
                customerPet = new CustomerPetModel
                {
                    CustomerPetId = id
                };

                context.CustomerPets.Add(customerPet);
            }

            customerPet.CustomerId =
                GetCustomer(context, customerEmail).CustomerId;

            customerPet.PetId = parsedPetId;
            customerPet.RelationshipType = relationship;
        }

        private static void SeedDiets(
            ApplicationDbContext context)
        {
            AddDiet(context, "71000000-0000-0000-0000-000000000001",
                1, "Purina Pro Plan Adult", "1.5 cups",
                FrequencyEnum.TwiceDaily, "Serve dry.");

            AddDiet(context, "71000000-0000-0000-0000-000000000002",
                3, "Hill's Science Diet Indoor", "1/2 cup",
                FrequencyEnum.TwiceDaily, "May add one tablespoon of water.");

            AddDiet(context, "71000000-0000-0000-0000-000000000003",
                5, "Parrot Pellet Blend", "1/3 cup",
                FrequencyEnum.OnceDaily, "Supplement with approved fresh produce.");

            AddDiet(context, "71000000-0000-0000-0000-000000000004",
                6, "Hill's Prescription Diet", "1 cup",
                FrequencyEnum.TwiceDaily, "No outside treats.");

            AddDiet(context, "71000000-0000-0000-0000-000000000005",
                8, "Royal Canin Labrador Adult", "1.5 cups",
                FrequencyEnum.TwiceDaily, null);

            AddDiet(context, "71000000-0000-0000-0000-000000000006",
                9, "Royal Canin Labrador Adult", "1.25 cups",
                FrequencyEnum.TwiceDaily, null);

            AddDiet(context, "71000000-0000-0000-0000-000000000007",
                10, "Timothy Hay", "Unlimited",
                FrequencyEnum.Other, "Refresh hay and water throughout the day.");

            AddDiet(context, "71000000-0000-0000-0000-000000000008",
                12, "Royal Canin Urinary SO", "3/4 cup",
                FrequencyEnum.TwiceDaily, "Prescription diet only.");

            AddDiet(context, "71000000-0000-0000-0000-000000000009",
                13, "Purina Pro Plan Giant Breed", "3 cups",
                FrequencyEnum.TwiceDaily, "Use elevated food bowl.");

            AddDiet(context, "71000000-0000-0000-0000-000000000010",
                16, "Limited Ingredient Salmon", "3/4 cup",
                FrequencyEnum.TwiceDaily, "Avoid chicken products.");

            AddDiet(context, "71000000-0000-0000-0000-000000000011",
                17, "Bearded Dragon Diet", "Per care sheet",
                FrequencyEnum.OnceDaily, "Greens daily; insects on scheduled days.");

            AddDiet(context, "71000000-0000-0000-0000-000000000012",
                18, "Senior Small Breed Formula", "3/4 cup",
                FrequencyEnum.TwiceDaily, "Soften with warm water.");

            AddDiet(context, "71000000-0000-0000-0000-000000000013",
                23, "Equine Senior Feed", "4 pounds",
                FrequencyEnum.TwiceDaily, "Hay provided separately.");

            AddDiet(context, "71000000-0000-0000-0000-000000000014",
                24, "Timothy Hay and Greens", "Per care sheet",
                FrequencyEnum.TwiceDaily, "Fresh water available at all times.");

            context.SaveChanges();
        }

        private static void AddDiet(
            ApplicationDbContext context,
            string dietId,
            int petNumber,
            string foodName,
            string amount,
            FrequencyEnum frequency,
            string notes)
        {
            Guid id = Guid.Parse(dietId);

            DietModel diet =
                context.Diets.FirstOrDefault(x => x.DietId == id);

            if (diet == null)
            {
                diet = new DietModel
                {
                    DietId = id
                };

                context.Diets.Add(diet);
            }

            diet.PetId = PetId(petNumber);
            diet.FoodName = foodName;
            diet.Amount = amount;
            diet.Frequency = frequency;
            diet.Notes = notes;
        }

        private static void SeedMedications(
            ApplicationDbContext context)
        {
            DateTime today = DateTime.Today;

            AddMedication(context,
                "72000000-0000-0000-0000-000000000001",
                6, "Levothyroxine", "0.4 mg",
                MedicationRouteEnum.Oral,
                FrequencyEnum.TwiceDaily,
                today.AddMonths(-8),
                null,
                "Give 30 minutes before feeding.");

            AddMedication(context,
                "72000000-0000-0000-0000-000000000002",
                18, "Cyclosporine Eye Drops", "1 drop each eye",
                MedicationRouteEnum.Ophthalmic,
                FrequencyEnum.TwiceDaily,
                today.AddMonths(-3),
                null,
                "Wash hands before and after administration.");

            AddMedication(context,
                "72000000-0000-0000-0000-000000000003",
                20, "Mometamax", "4 drops",
                MedicationRouteEnum.Otic,
                FrequencyEnum.OnceDaily,
                today.AddDays(-3),
                today.AddDays(7),
                "Administer only during active ear flare-up.");

            AddMedication(context,
                "72000000-0000-0000-0000-000000000004",
                4, "Carprofen", "75 mg",
                MedicationRouteEnum.Oral,
                FrequencyEnum.AsNeeded,
                today.AddMonths(-2),
                null,
                "Give with food for arthritis discomfort.");

            AddMedication(context,
                "72000000-0000-0000-0000-000000000005",
                12, "Prazosin", "0.5 mg",
                MedicationRouteEnum.Oral,
                FrequencyEnum.TwiceDaily,
                today.AddMonths(-1),
                today.AddMonths(2),
                "Prescription urinary support.");

            context.SaveChanges();
        }

        private static void AddMedication(
            ApplicationDbContext context,
            string medicationId,
            int petNumber,
            string medicationName,
            string dosage,
            MedicationRouteEnum route,
            FrequencyEnum frequency,
            DateTime startDate,
            DateTime? endDate,
            string notes)
        {
            Guid id = Guid.Parse(medicationId);

            MedicationModel medication =
                context.Medications.FirstOrDefault(
                    x => x.MedicationId == id);

            if (medication == null)
            {
                medication = new MedicationModel
                {
                    MedicationId = id
                };

                context.Medications.Add(medication);
            }

            medication.PetId = PetId(petNumber);
            medication.MedicationName = medicationName;
            medication.Dosage = dosage;
            medication.Route = route;
            medication.Frequency = frequency;
            medication.StartDate = startDate;
            medication.EndDate = endDate;
            medication.Notes = notes;
        }

        private static void SeedPetVaccines(
            ApplicationDbContext context)
        {
            DateTime today = DateTime.Today;
            int recordNumber = 1;

            AddDogVaccines(context, 1, today.AddMonths(8),
                ref recordNumber);

            AddDogVaccines(context, 2, today.AddMonths(5),
                ref recordNumber);

            AddCatVaccines(context, 3, today.AddMonths(9),
                ref recordNumber);

            AddDogVaccines(context, 4, today.AddMonths(-1),
                ref recordNumber);

            AddVaccineRecord(context, 5, "Polyomavirus",
                SpeciesEnum.Bird, today.AddMonths(-4),
                today.AddMonths(8), ref recordNumber);

            AddDogVaccines(context, 6, today.AddMonths(3),
                ref recordNumber);

            AddCatVaccines(context, 7, today.AddMonths(11),
                ref recordNumber);

            AddDogVaccines(context, 8, today.AddMonths(7),
                ref recordNumber);

            AddDogVaccines(context, 9, today.AddMonths(7),
                ref recordNumber);

            AddVaccineRecord(context, 10, "RHDV2",
                SpeciesEnum.Rabbit, today.AddMonths(-6),
                today.AddMonths(6), ref recordNumber);

            AddDogVaccines(context, 11, today.AddMonths(2),
                ref recordNumber);

            AddCatVaccines(context, 12, today.AddMonths(-2),
                ref recordNumber);

            AddDogVaccines(context, 13, today.AddMonths(10),
                ref recordNumber);

            AddVaccineRecord(context, 14, "Polyomavirus",
                SpeciesEnum.Bird, today.AddMonths(-3),
                today.AddMonths(9), ref recordNumber);

            AddDogVaccines(context, 15, today.AddMonths(4),
                ref recordNumber);

            AddDogVaccines(context, 16, today.AddMonths(1),
                ref recordNumber);

            AddDogVaccines(context, 18, today.AddMonths(-3),
                ref recordNumber);

            AddCatVaccines(context, 19, today.AddMonths(6),
                ref recordNumber);

            AddDogVaccines(context, 20, today.AddMonths(2),
                ref recordNumber);

            AddDogVaccines(context, 21, today.AddMonths(12),
                ref recordNumber);

            AddCatVaccines(context, 22, today.AddMonths(10),
                ref recordNumber);

            AddHorseVaccines(context, 23, today.AddMonths(5),
                ref recordNumber);

            AddVaccineRecord(context, 24, "RHDV2",
                SpeciesEnum.Rabbit, today.AddMonths(-5),
                today.AddMonths(7), ref recordNumber);

            context.SaveChanges();
        }

        private static void AddDogVaccines(
            ApplicationDbContext context,
            int petNumber,
            DateTime commonExpiration,
            ref int recordNumber)
        {
            DateTime dateGiven = commonExpiration.AddYears(-1);

            AddVaccineRecord(context, petNumber, "Rabies",
                SpeciesEnum.Dog, dateGiven, commonExpiration,
                ref recordNumber);

            AddVaccineRecord(context, petNumber, "DHPP",
                SpeciesEnum.Dog, dateGiven, commonExpiration,
                ref recordNumber);

            AddVaccineRecord(context, petNumber, "Bordetella",
                SpeciesEnum.Dog, dateGiven, commonExpiration,
                ref recordNumber);
        }

        private static void AddCatVaccines(
            ApplicationDbContext context,
            int petNumber,
            DateTime commonExpiration,
            ref int recordNumber)
        {
            DateTime dateGiven = commonExpiration.AddYears(-1);

            AddVaccineRecord(context, petNumber, "Rabies",
                SpeciesEnum.Cat, dateGiven, commonExpiration,
                ref recordNumber);

            AddVaccineRecord(context, petNumber, "FVRCP",
                SpeciesEnum.Cat, dateGiven, commonExpiration,
                ref recordNumber);
        }

        private static void AddHorseVaccines(
            ApplicationDbContext context,
            int petNumber,
            DateTime commonExpiration,
            ref int recordNumber)
        {
            DateTime dateGiven = commonExpiration.AddYears(-1);

            AddVaccineRecord(context, petNumber, "Rabies",
                SpeciesEnum.Horse, dateGiven, commonExpiration,
                ref recordNumber);

            AddVaccineRecord(context, petNumber, "Tetanus",
                SpeciesEnum.Horse, dateGiven, commonExpiration,
                ref recordNumber);

            AddVaccineRecord(context, petNumber, "West Nile Virus",
                SpeciesEnum.Horse, dateGiven, commonExpiration,
                ref recordNumber);

            AddVaccineRecord(context, petNumber, "EEE/WEE",
                SpeciesEnum.Horse, dateGiven, commonExpiration,
                ref recordNumber);
        }

        private static void AddVaccineRecord(
            ApplicationDbContext context,
            int petNumber,
            string vaccineName,
            SpeciesEnum species,
            DateTime dateGiven,
            DateTime expirationDate,
            ref int recordNumber)
        {
            VaccineModel vaccine =
                context.Vaccines.FirstOrDefault(
                    x => x.VaccineName == vaccineName &&
                         x.Species == species);

            if (vaccine == null)
            {
                throw new InvalidOperationException(
                    "Seed vaccine was not found: " +
                    vaccineName +
                    " (" +
                    species +
                    ").");
            }

            Guid id = Guid.Parse(
                "73000000-0000-0000-0000-" +
                recordNumber.ToString("000000000000"));

            PetVaccineModel petVaccine =
                context.PetVaccines.FirstOrDefault(
                    x => x.PetVaccineId == id);

            if (petVaccine == null)
            {
                petVaccine = new PetVaccineModel
                {
                    PetVaccineId = id
                };

                context.PetVaccines.Add(petVaccine);
            }

            petVaccine.PetId = PetId(petNumber);
            petVaccine.VaccineId = vaccine.VaccineId;
            petVaccine.DateGiven = dateGiven;
            petVaccine.ExpirationDate = expirationDate;
            petVaccine.DocumentFilePath =
                "~/Content/Documents/Seed/Vaccines/" +
                "seed-vaccine-record.pdf";

            petVaccine.Notes =
                expirationDate.Date < DateTime.Today
                    ? "Seeded expired vaccination record for compliance testing."
                    : "Seeded vaccination record.";

            recordNumber++;
        }

        private static CustomerModel GetCustomer(
            ApplicationDbContext context,
            string email)
        {
            CustomerModel customer =
                context.Customers.FirstOrDefault(
                    x => x.Email == email);

            if (customer == null)
            {
                throw new InvalidOperationException(
                    "Seed customer was not found: " + email);
            }

            return customer;
        }

        private static Guid PetId(int petNumber)
        {
            return Guid.Parse(
                "70000000-0000-0000-0000-" +
                petNumber.ToString("000000000000"));
        }
    }
}
