using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JamesPetBoarding.Data
{
    public static partial class DatabaseSeeder
    {
        private static void SeedTransactions(ApplicationDbContext context)
        {
            SeedBoardings(context);
            SeedInvoicesAndItems(context);
            SeedPayments(context);
            context.SaveChanges();
        }

        private static void SeedBoardings(ApplicationDbContext context)
        {
            DateTime today = DateTime.Today;
            EmployeeModel frontDesk = GetEmployeeByRole(context, EmployeeRoleEnum.FrontDesk);
            EmployeeModel supervisor = GetEmployeeByRole(context, EmployeeRoleEnum.Supervisor);

            // Five completed stays for Bella provide a frequent-customer report scenario.
            AddBoarding(context, 1, 1, -330, -327, BoardingStatusEnum.CheckedOut, 0, frontDesk, supervisor, null);
            AddBoarding(context, 2, 1, -260, -257, BoardingStatusEnum.CheckedOut, 1, frontDesk, supervisor, null);
            AddBoarding(context, 3, 1, -190, -186, BoardingStatusEnum.CheckedOut, 2, frontDesk, supervisor, null);
            AddBoarding(context, 4, 1, -120, -117, BoardingStatusEnum.CheckedOut, 3, frontDesk, supervisor, null);
            AddBoarding(context, 5, 1, -55, -51, BoardingStatusEnum.CheckedOut, 4, frontDesk, supervisor, null);

            AddBoarding(context, 6, 2, -95, -91, BoardingStatusEnum.CheckedOut, 5, frontDesk, supervisor, null);
            AddBoarding(context, 7, 3, -75, -72, BoardingStatusEnum.CheckedOut, 0, frontDesk, supervisor, null);
            AddBoarding(context, 8, 6, -32, -28, BoardingStatusEnum.CheckedOut, 6, frontDesk, supervisor, null);
            AddBoarding(context, 9, 8, -18, -14, BoardingStatusEnum.CheckedOut, 7, frontDesk, supervisor, null);
            AddBoarding(context, 10, 12, -14, -11, BoardingStatusEnum.CheckedOut, 1, frontDesk, supervisor, null);

            // Current boarders.
            AddBoarding(context, 11, 9, -2, 2, BoardingStatusEnum.CheckedIn, 8, frontDesk, null, null);
            AddBoarding(context, 12, 13, -1, 3, BoardingStatusEnum.CheckedIn, 9, frontDesk, null, null);
            AddBoarding(context, 13, 14, 0, 4, BoardingStatusEnum.CheckedIn, 0, frontDesk, null, null);

            // Future reservations.
            AddBoarding(context, 14, 15, 5, 8, BoardingStatusEnum.Confirmed, 10, null, null, null);
            AddBoarding(context, 15, 16, 10, 14, BoardingStatusEnum.Confirmed, 11, null, null, null);
            AddBoarding(context, 16, 21, 18, 23, BoardingStatusEnum.Scheduled, 12, null, null, null);
            AddBoarding(context, 17, 22, 30, 34, BoardingStatusEnum.Scheduled, 2, null, null, null);

            // Exception workflows.
            AddBoarding(context, 18, 4, -20, -17, BoardingStatusEnum.Cancelled, 3, null, null,
                "Customer cancelled because travel plans changed.");
            AddBoarding(context, 19, 11, -10, -7, BoardingStatusEnum.NoShow, 4, null, null,
                "Customer did not arrive and could not be reached.");
            AddBoarding(context, 20, 24, 45, 49, BoardingStatusEnum.Confirmed, 5, null, null, null);

            context.SaveChanges();
        }

        private static void AddBoarding(
            ApplicationDbContext context,
            int boardingNumber,
            int petNumber,
            int startDayOffset,
            int endDayOffset,
            BoardingStatusEnum status,
            int unitOffset,
            EmployeeModel checkInEmployee,
            EmployeeModel checkOutEmployee,
            string exceptionReason)
        {
            Guid id = BoardingId(boardingNumber);
            BoardingModel boarding = context.Boardings.FirstOrDefault(x => x.BoardingId == id);

            if (boarding == null)
            {
                boarding = new BoardingModel { BoardingId = id };
                context.Boardings.Add(boarding);
            }

            Guid petId = PetId(petNumber);
            CustomerPetModel owner = context.CustomerPets.First(x =>
                x.PetId == petId && x.RelationshipType == RelationshipTypeEnum.Owner);

            PetModel pet = context.Pets.First(x => x.PetId == petId);
            BoardingUnitModel unit = GetBoardingUnit(context, pet.Species, unitOffset);
            DateTime start = DateTime.Today.AddDays(startDayOffset).AddHours(9);
            DateTime end = DateTime.Today.AddDays(endDayOffset).AddHours(17);

            boarding.CustomerId = owner.CustomerId;
            boarding.PetId = petId;
            boarding.BoardingUnitId = unit.BoardingUnitId;
            boarding.StartDateTime = start;
            boarding.EndDateTime = end;
            boarding.Status = status;
            boarding.Notes = "Seeded boarding record for application and report testing.";

            boarding.ActualCheckInDateTime = null;
            boarding.CheckedInByEmployeeId = null;
            boarding.ActualCheckOutDateTime = null;
            boarding.CheckedOutByEmployeeId = null;
            boarding.CancelledDateTime = null;
            boarding.CancelledByEmployeeId = null;
            boarding.CancelledReason = null;
            boarding.NoShowDateTime = null;
            boarding.NoShowByEmployeeId = null;

            if (status == BoardingStatusEnum.CheckedIn || status == BoardingStatusEnum.CheckedOut)
            {
                boarding.ActualCheckInDateTime = start.AddMinutes(-10);
                boarding.CheckedInByEmployeeId = checkInEmployee.EmployeeId;
            }

            if (status == BoardingStatusEnum.CheckedOut)
            {
                boarding.ActualCheckOutDateTime = end.AddMinutes(-20);
                boarding.CheckedOutByEmployeeId = checkOutEmployee.EmployeeId;
            }

            if (status == BoardingStatusEnum.Cancelled)
            {
                boarding.CancelledDateTime = start.AddDays(-5);
                boarding.CancelledByEmployeeId = GetEmployeeByRole(context, EmployeeRoleEnum.Manager).EmployeeId;
                boarding.CancelledReason = exceptionReason;
            }

            if (status == BoardingStatusEnum.NoShow)
            {
                boarding.NoShowDateTime = start.AddHours(2);
                boarding.NoShowByEmployeeId = GetEmployeeByRole(context, EmployeeRoleEnum.Supervisor).EmployeeId;
                boarding.Notes = exceptionReason;
            }
        }

        private static BoardingUnitModel GetBoardingUnit(
            ApplicationDbContext context,
            SpeciesEnum species,
            int offset)
        {
            UnitTypeEnum preferredType;

            switch (species)
            {
                case SpeciesEnum.Cat:
                    preferredType = UnitTypeEnum.CatCondo;
                    break;
                case SpeciesEnum.Bird:
                    preferredType = UnitTypeEnum.BirdCage;
                    break;
                case SpeciesEnum.Rabbit:
                case SpeciesEnum.Reptile:
                    preferredType = UnitTypeEnum.SmallAnimalEnclosure;
                    break;
                case SpeciesEnum.Horse:
                    preferredType = UnitTypeEnum.LargeAnimalEnclosure;
                    break;
                default:
                    preferredType = UnitTypeEnum.Kennel;
                    break;
            }

            List<BoardingUnitModel> units = context.BoardingUnits
                .Where(x => x.IsActive && x.UnitType == preferredType)
                .OrderBy(x => x.UnitName)
                .ThenBy(x => x.UnitNumber)
                .ToList();

            if (!units.Any() && species == SpeciesEnum.Dog)
            {
                units = context.BoardingUnits
                    .Where(x => x.IsActive && x.UnitType == UnitTypeEnum.Suite)
                    .OrderBy(x => x.UnitName)
                    .ThenBy(x => x.UnitNumber)
                    .ToList();
            }

            if (!units.Any())
            {
                throw new InvalidOperationException("No compatible seeded boarding unit was found for " + species + ".");
            }

            return units[offset % units.Count];
        }

        private static void SeedInvoicesAndItems(ApplicationDbContext context)
        {
            DateTime today = DateTime.Today;

            AddInvoice(context, 1, 1, 1, InvoiceTypeEnum.BoardingOnly, today.AddDays(-327), InvoiceStatusEnum.PaidInFull,
                265.00m, 21.20m, 10.00m, 276.20m, 276.20m, 0.00m, null, null);
            AddBoardingItem(context, 1, 1, 1, "Three-night boarding", 3, 80.00m);
            AddServiceItem(context, 2, 1, 1, ServiceNameEnum.ExtraPlayTime, SpeciesEnum.Dog, "Extra play time", 1, 25.00m);

            AddInvoice(context, 2, 2, 6, InvoiceTypeEnum.BoardingOnly, today.AddDays(-91), InvoiceStatusEnum.PaidInFull,
                180.00m, 14.40m, 0.00m, 194.40m, 194.40m, 0.00m, null, null);
            AddBoardingItem(context, 3, 2, 6, "Four-night boarding", 4, 45.00m);

            AddInvoice(context, 3, 3, null, InvoiceTypeEnum.ServiceOnly, today.AddDays(-40), InvoiceStatusEnum.PartiallyPaid,
                90.00m, 7.20m, 0.00m, 97.20m, 50.00m, 47.20m, null, null);
            AddServiceItem(context, 4, 3, null, ServiceNameEnum.FullGrooming, SpeciesEnum.Cat, "Full grooming", 1, 70.00m);
            AddServiceItem(context, 5, 3, null, ServiceNameEnum.NailTrim, SpeciesEnum.Cat, "Nail trim", 1, 20.00m);

            AddInvoice(context, 4, 4, 4, InvoiceTypeEnum.BoardingOnly, today.AddDays(-117), InvoiceStatusEnum.Overdue,
                300.00m, 24.00m, 0.00m, 324.00m, 0.00m, 324.00m, null, null);
            AddBoardingItem(context, 6, 4, 4, "Three-night suite boarding", 3, 100.00m);

            AddInvoice(context, 5, 6, null, InvoiceTypeEnum.ServiceOnly, today.AddDays(-12), InvoiceStatusEnum.Issued,
                45.00m, 3.60m, 0.00m, 48.60m, 0.00m, 48.60m, null, null);
            AddServiceItem(context, 7, 5, null, ServiceNameEnum.Bath, SpeciesEnum.Dog, "Bath service", 1, 45.00m);

            AddInvoice(context, 6, 7, null, InvoiceTypeEnum.ServiceOnly, today, InvoiceStatusEnum.Draft,
                35.00m, 2.80m, 0.00m, 37.80m, 0.00m, 37.80m, null, null);
            AddServiceItem(context, 8, 6, null, ServiceNameEnum.EarCleaning, SpeciesEnum.Cat, "Ear cleaning", 1, 35.00m);

            AddInvoice(context, 7, 8, 9, InvoiceTypeEnum.BoardingOnly, today.AddDays(-14), InvoiceStatusEnum.Void,
                150.00m, 12.00m, 0.00m, 162.00m, 0.00m, 0.00m,
                TransactionVoidReasonEnum.DuplicateInvoice, "Duplicate invoice created during checkout.");
            AddBoardingItem(context, 9, 7, 9, "Boarding charge", 3, 50.00m);

            AddInvoice(context, 8, 9, 9, InvoiceTypeEnum.BoardingOnly, today.AddDays(-14), InvoiceStatusEnum.PartiallyPaid,
                245.00m, 19.60m, 0.00m, 264.60m, 80.00m, 184.60m, null, null);
            AddBoardingItem(context, 10, 8, 9, "Four-night boarding", 4, 55.00m);
            AddServiceItem(context, 11, 8, 9, ServiceNameEnum.MedicationAdministration, SpeciesEnum.Dog,
                "Medication administration", 1, 25.00m);

            AddInvoice(context, 9, 15, null, InvoiceTypeEnum.ServiceOnly, today.AddDays(-20), InvoiceStatusEnum.PaidInFull,
                65.00m, 5.20m, 0.00m, 70.20m, 70.20m, 0.00m, null, null);
            AddServiceItem(context, 12, 9, null, ServiceNameEnum.TrainingSession, SpeciesEnum.Dog,
                "Private training session", 1, 65.00m);

            AddInvoice(context, 10, 12, 10, InvoiceTypeEnum.BoardingOnly, today.AddDays(-11), InvoiceStatusEnum.PaidInFull,
                360.00m, 27.20m, 20.00m, 367.20m, 367.20m, 0.00m, null, null);
            AddBoardingItem(context, 13, 10, 10, "Three-night boarding", 3, 120.00m);

            AddInvoice(context, 11, 9, 11, InvoiceTypeEnum.BoardingOnly, today, InvoiceStatusEnum.Issued,
                200.00m, 16.00m, 0.00m, 216.00m, 0.00m, 216.00m, null, null);
            AddBoardingItem(context, 14, 11, 11, "Current boarding deposit and stay", 4, 50.00m);

            AddInvoice(context, 12, 15, 14, InvoiceTypeEnum.BoardingOnly, today, InvoiceStatusEnum.Draft,
                150.00m, 12.00m, 0.00m, 162.00m, 0.00m, 162.00m, null, null);
            AddBoardingItem(context, 15, 12, 14, "Future boarding estimate", 3, 50.00m);

            AddInvoice(context, 13, 22, null, InvoiceTypeEnum.ServiceOnly, today.AddDays(-8), InvoiceStatusEnum.PaidInFull,
                65.00m, 5.20m, 0.00m, 70.20m, 70.20m, 0.00m, null, null);
            AddServiceItem(context, 16, 13, null, ServiceNameEnum.FullGrooming, SpeciesEnum.Cat,
                "Full grooming", 1, 65.00m);

            AddInvoice(context, 14, 21, 16, InvoiceTypeEnum.BoardingOnly, today.AddDays(-45), InvoiceStatusEnum.Overdue,
                280.00m, 22.40m, 0.00m, 302.40m, 100.00m, 202.40m, null, null);
            AddBoardingItem(context, 17, 14, 16, "Boarding reservation charges", 4, 70.00m);

            context.SaveChanges();
        }

        private static void AddInvoice(
            ApplicationDbContext context,
            int invoiceNumber,
            int petNumber,
            int? boardingNumber,
            InvoiceTypeEnum invoiceType,
            DateTime invoiceDate,
            InvoiceStatusEnum status,
            decimal subtotal,
            decimal tax,
            decimal discount,
            decimal total,
            decimal paid,
            decimal balance,
            TransactionVoidReasonEnum? voidReason,
            string voidNotes)
        {
            Guid id = InvoiceId(invoiceNumber);
            InvoiceModel invoice = context.Invoices.FirstOrDefault(x => x.InvoiceId == id);

            if (invoice == null)
            {
                invoice = new InvoiceModel { InvoiceId = id };
                context.Invoices.Add(invoice);
            }

            Guid petId = PetId(petNumber);
            CustomerPetModel owner = context.CustomerPets.First(x =>
                x.PetId == petId && x.RelationshipType == RelationshipTypeEnum.Owner);

            invoice.CustomerId = owner.CustomerId;
            invoice.PetId = petId;
            invoice.BoardingId = boardingNumber.HasValue ? BoardingId(boardingNumber.Value) : (Guid?)null;
            invoice.InvoiceType = invoiceType;
            invoice.InvoiceDateTime = invoiceDate.AddHours(17);
            invoice.InvoiceStatus = status;
            invoice.Subtotal = subtotal;
            invoice.TaxAmount = tax;
            invoice.DiscountAmount = discount;
            invoice.TotalAmount = total;
            invoice.AmountPaid = paid;
            invoice.Balance = balance;
            invoice.Notes = "Seeded invoice for application and report testing.";
            invoice.VoidReason = voidReason;
            invoice.VoidNotes = voidNotes;
            invoice.VoidDateTime = voidReason.HasValue ? invoice.InvoiceDateTime.AddMinutes(30) : (DateTime?)null;
            invoice.VoidedByEmployeeId = voidReason.HasValue
                ? GetEmployeeByRole(context, EmployeeRoleEnum.Admin).EmployeeId
                : (Guid?)null;
        }

        private static void AddBoardingItem(
            ApplicationDbContext context,
            int itemNumber,
            int invoiceNumber,
            int boardingNumber,
            string description,
            int quantity,
            decimal unitPrice)
        {
            AddInvoiceItem(context, itemNumber, invoiceNumber, boardingNumber, null,
                InvoiceItemTypeEnum.Boarding, description, quantity, unitPrice);
        }

        private static void AddServiceItem(
            ApplicationDbContext context,
            int itemNumber,
            int invoiceNumber,
            int? boardingNumber,
            ServiceNameEnum serviceName,
            SpeciesEnum species,
            string description,
            int quantity,
            decimal unitPrice)
        {
            ServiceModel service = context.Services.FirstOrDefault(x =>
                x.ServiceName == serviceName && x.Species == species);

            if (service == null)
            {
                service = context.Services.FirstOrDefault(x => x.ServiceName == serviceName);
            }

            if (service == null)
            {
                throw new InvalidOperationException("Seed service was not found: " + serviceName + ".");
            }

            AddInvoiceItem(context, itemNumber, invoiceNumber, boardingNumber,
                service.ServiceId, InvoiceItemTypeEnum.Service, description, quantity, unitPrice);
        }

        private static void AddInvoiceItem(
            ApplicationDbContext context,
            int itemNumber,
            int invoiceNumber,
            int? boardingNumber,
            Guid? serviceId,
            InvoiceItemTypeEnum itemType,
            string description,
            int quantity,
            decimal unitPrice)
        {
            Guid id = InvoiceItemId(itemNumber);
            InvoiceItemModel item = context.InvoiceItems.FirstOrDefault(x => x.InvoiceItemId == id);

            if (item == null)
            {
                item = new InvoiceItemModel { InvoiceItemId = id };
                context.InvoiceItems.Add(item);
            }

            item.InvoiceId = InvoiceId(invoiceNumber);
            item.BoardingId = boardingNumber.HasValue ? BoardingId(boardingNumber.Value) : (Guid?)null;
            item.ServiceId = serviceId;
            item.ItemType = itemType;
            item.Description = description;
            item.Quantity = quantity;
            item.UnitPrice = unitPrice;
            item.LineTotal = quantity * unitPrice;
            item.Notes = "Seeded invoice item.";
        }

        private static void SeedPayments(ApplicationDbContext context)
        {
            EmployeeModel processor = GetEmployeeByRole(context, EmployeeRoleEnum.FrontDesk);
            EmployeeModel manager = GetEmployeeByRole(context, EmployeeRoleEnum.Manager);

            AddPayment(context, 1, 1, -327, PaymentMethodEnum.CreditCard, 276.20m,
                processor, false, null, null, null);
            AddPayment(context, 2, 2, -91, PaymentMethodEnum.DebitCard, 194.40m,
                processor, false, null, null, null);
            AddPayment(context, 3, 3, -40, PaymentMethodEnum.Cash, 50.00m,
                processor, false, null, null, null);
            AddPayment(context, 4, 8, -14, PaymentMethodEnum.CreditCard, 80.00m,
                processor, false, null, null, null);
            AddPayment(context, 5, 8, -14, PaymentMethodEnum.CreditCard, 184.60m,
                processor, true, TransactionVoidReasonEnum.DuplicatePayment, manager,
                "Duplicate payment was entered during checkout.");
            AddPayment(context, 6, 9, -20, PaymentMethodEnum.ApplePay, 70.20m,
                processor, false, null, null, null);
            AddPayment(context, 7, 10, -11, PaymentMethodEnum.ACH, 367.20m,
                processor, false, null, null, null);
            AddPayment(context, 8, 13, -8, PaymentMethodEnum.GooglePay, 70.20m,
                processor, false, null, null, null);
            AddPayment(context, 9, 14, -44, PaymentMethodEnum.Check, 100.00m,
                processor, false, null, null, null);

            context.SaveChanges();
        }

        private static void AddPayment(
            ApplicationDbContext context,
            int paymentNumber,
            int invoiceNumber,
            int paymentDayOffset,
            PaymentMethodEnum method,
            decimal amount,
            EmployeeModel processor,
            bool isVoided,
            TransactionVoidReasonEnum? voidReason,
            EmployeeModel voidedBy,
            string notes)
        {
            Guid id = PaymentId(paymentNumber);
            PaymentModel payment = context.Payments.FirstOrDefault(x => x.PaymentId == id);

            if (payment == null)
            {
                payment = new PaymentModel { PaymentId = id };
                context.Payments.Add(payment);
            }

            DateTime paymentDate = DateTime.Today.AddDays(paymentDayOffset).AddHours(18);
            payment.InvoiceId = InvoiceId(invoiceNumber);
            payment.PaymentDateTime = paymentDate;
            payment.PaymentMethod = method;
            payment.Amount = amount;
            payment.TransactionReference = "SEED-" + paymentNumber.ToString("0000");
            payment.ProcessedByEmployeeId = processor.EmployeeId;
            payment.IsVoided = isVoided;
            payment.VoidedDateTime = isVoided ? paymentDate.AddMinutes(20) : (DateTime?)null;
            payment.VoidReason = voidReason;
            payment.VoidedByEmployeeId = isVoided ? voidedBy.EmployeeId : (Guid?)null;
            payment.Notes = notes ?? "Seeded payment record.";
        }

        private static EmployeeModel GetEmployeeByRole(
            ApplicationDbContext context,
            EmployeeRoleEnum role)
        {
            EmployeeModel employee = context.Employees
                .Where(x => x.IsActive && x.Role == role)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .FirstOrDefault();

            if (employee == null)
            {
                throw new InvalidOperationException("No active seeded employee was found for role " + role + ".");
            }

            return employee;
        }

        private static Guid BoardingId(int number)
        {
            return SeedGuid("80000000-0000-0000-0000-", number);
        }

        private static Guid InvoiceId(int number)
        {
            return SeedGuid("81000000-0000-0000-0000-", number);
        }

        private static Guid InvoiceItemId(int number)
        {
            return SeedGuid("82000000-0000-0000-0000-", number);
        }

        private static Guid PaymentId(int number)
        {
            return SeedGuid("83000000-0000-0000-0000-", number);
        }

        private static Guid SeedGuid(string prefix, int number)
        {
            return Guid.Parse(prefix + number.ToString("000000000000"));
        }
    }
}
