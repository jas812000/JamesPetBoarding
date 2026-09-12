using JamesPetBoarding.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Services
{
    public class PdfReportService
    {
        public byte[] GenerateCustomerActivityReportPdf(CustomerActivityReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Customer Activity Report").FontSize(20).Bold();

                    page.Content()
                        .PaddingVertical(15)
                        .Column(column =>
                        {
                            column.Item()
                                .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                            column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                            CustomerActivityReportFilterVM filter = report.CustomerActivityReportFilter;

                            column.Item().Text(
                                "Customer: " + (filter.CustomerId.HasValue
                                    ? filter.CustomerId.Value.ToString()
                                    : "All"));

                            column.Item().Text(
                                "Active Status: " + (filter.ActiveStatus.HasValue
                                    ? filter.ActiveStatus.Value.ToString()
                                    : "All"));

                            column.Item().Text(
                                "First Name: " + (!string.IsNullOrWhiteSpace(filter.FirstName)
                                    ? filter.FirstName
                                    : "All"));

                            column.Item().Text(
                                "Last Name: " + (!string.IsNullOrWhiteSpace(filter.LastName)
                                    ? filter.LastName
                                    : "All"));

                            column.Item().Text(
                                "City: " + (!string.IsNullOrWhiteSpace(filter.City)
                                    ? filter.City
                                    : "All"));

                            column.Item().Text(
                                "State: " + (filter.State.HasValue
                                    ? filter.State.Value.ToString()
                                    : "All"));

                            column.Item().Text(
                                "ZipCode: " + (!string.IsNullOrWhiteSpace(filter.ZipCode)
                                    ? filter.ZipCode
                                    : "All"));

                            column.Item().Text(
                                "Phone: " + (!string.IsNullOrWhiteSpace(filter.Phone)
                                    ? filter.Phone
                                    : "All"));

                            column.Item().Text(
                                "Email: " + (!string.IsNullOrWhiteSpace(filter.Email)
                                    ? filter.Email
                                    : "All"));


                            column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                            column.Item().Text("Total Customers: " + report.TotalCount);

                            column.Item().Text("Active Customers: " + report.ActiveCount);

                            column.Item().Text("Inactive Customers: " + report.InactiveCount);

                            column.Item().PaddingTop(15).Text("Customer Activity").FontSize(16).Bold();

                            column.Item()
                                .PaddingTop(5)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2.2f);
                                        columns.RelativeColumn(1.1f);
                                        columns.RelativeColumn(0.7f);
                                        columns.RelativeColumn(0.9f);
                                        columns.RelativeColumn(1.4f);
                                        columns.RelativeColumn(0.9f);
                                        columns.RelativeColumn(1.4f);
                                        columns.RelativeColumn(1.3f);
                                        columns.RelativeColumn(1.4f);
                                        columns.RelativeColumn(1.4f);
                                        columns.RelativeColumn(1.0f);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Customer").FontSize(8).Bold();
                                        header.Cell().Text("Status").FontSize(8).Bold();
                                        header.Cell().Text("Pets").FontSize(8).Bold();
                                        header.Cell().Text("Boardings").FontSize(8).Bold();
                                        header.Cell().Text("Last Boarding").FontSize(8).Bold();
                                        header.Cell().Text("Invoices").FontSize(8).Bold();
                                        header.Cell().Text("Invoiced").FontSize(8).Bold();
                                        header.Cell().Text("Total Paid").FontSize(8).Bold();
                                        header.Cell().Text("Outstanding").FontSize(8).Bold();
                                        header.Cell().Text("Last Activity").FontSize(8).Bold();
                                        header.Cell().Text("Frequent").FontSize(8).Bold();
                                    });

                                    foreach (CustomerActivityReportRowVM row in report.CustomerActivityReportRows)
                                    {
                                        table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                        table.Cell().Text(row.ActiveStatusDisplay).FontSize(8);
                                        table.Cell().Text(row.PetCount.ToString()).FontSize(8);
                                        table.Cell().Text(row.BoardingCount.ToString()).FontSize(8);
                                        table.Cell().Text(row.LastBoardingDateDisplay).FontSize(8);
                                        table.Cell().Text(row.InvoiceCount.ToString()).FontSize(8);
                                        table.Cell().Text(row.TotalInvoiceAmountDisplay).FontSize(8);
                                        table.Cell().Text(row.TotalPaymentAmountDisplay).FontSize(8);
                                        table.Cell().Text(row.OutstandingBalanceDisplay).FontSize(8);
                                        table.Cell().Text(row.LastActivityDateDisplay).FontSize(8);
                                        table.Cell().Text(row.IsFrequentCustomerDisplay).FontSize(8);
                                    }

                                });

                        });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();
                    });
                });
            });
            return document.GeneratePdf();
        }


        public byte[] GeneratePetReportPdf(PetReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Pet Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        PetReportFilterVM filter = report.PetReportFilter;

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Veterinarian: " + (filter.VetId.HasValue
                                ? filter.VetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Active Status: " + (filter.ActiveStatus.HasValue
                                ? filter.ActiveStatus.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet Name: " + (!string.IsNullOrWhiteSpace(filter.PetName)
                                ? filter.PetName
                                : "All"));

                        column.Item().Text(
                            "Species: " + (filter.Species.HasValue
                                ? filter.Species.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Breed: " + (!string.IsNullOrWhiteSpace(filter.Breed)
                                ? filter.Breed
                                : "All"));

                        column.Item().Text(
                            "Sex: " + (filter.Sex.HasValue
                                ? filter.Sex.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Birth Date: " + (filter.BirthDate.HasValue
                                ? filter.BirthDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text("Weight Unit: " + filter.WeightUnit.ToString());

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Total Pets: " + report.TotalCount);

                        column.Item().Text("Active Pets: " + report.ActiveCount);

                        column.Item().Text("Inactive Pets: " + report.InactiveCount);

                        column.Item().PaddingTop(15).Text("Pets").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1.4f);
                                    columns.RelativeColumn(1.0f);
                                    columns.RelativeColumn(1.8f);
                                    columns.RelativeColumn(1.8f);
                                    columns.RelativeColumn(1.0f);
                                    columns.RelativeColumn(1.4f);
                                    columns.RelativeColumn(0.8f);
                                    columns.RelativeColumn(1.2f);
                                    columns.RelativeColumn(0.8f);
                                    columns.RelativeColumn(1.1f);
                                    columns.RelativeColumn(2.2f);
                                });

                            table.Header(header =>
                            {
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Veterinarian").FontSize(8).Bold();
                                header.Cell().Text("Species").FontSize(8).Bold();
                                header.Cell().Text("Breed").FontSize(8).Bold();
                                header.Cell().Text("Sex").FontSize(8).Bold();
                                header.Cell().Text("Birth Date").FontSize(8).Bold();
                                header.Cell().Text("Age").FontSize(8).Bold();
                                header.Cell().Text("Weight").FontSize(8).Bold();
                                header.Cell().Text("Notes").FontSize(8).Bold();
                            });

                            foreach (PetReportRowVM row in report.PetReportRows)
                            {
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.ActiveStatusDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.VeterinarianNameDisplay).FontSize(8);
                                table.Cell().Text(row.SpeciesDisplay).FontSize(8);
                                table.Cell().Text(row.BreedDisplay).FontSize(8);
                                table.Cell().Text(row.SexDisplay).FontSize(8);
                                table.Cell().Text(row.BirthDateDisplay).FontSize(8);
                                table.Cell().Text(row.AgeDisplay).FontSize(8);
                                table.Cell().Text(row.WeightDisplay).FontSize(8);
                                table.Cell().Text(row.NotesDisplay).FontSize(8);
                            }
                        });
                    });
                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }


        public byte[] GeneratePaymentReportPdf(PaymentReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Payment Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        PaymentReportFilterVM filter = report.PaymentReportFilter;

                        column.Item().Text(
                            "Payment Start Date: " + (filter.PaymentStartDate.HasValue
                                ? filter.PaymentStartDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Payment End Date: " + (filter.PaymentEndDate.HasValue
                                ? filter.PaymentEndDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Invoice Type: " + (filter.InvoiceType.HasValue
                                ? filter.InvoiceType.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Payment Method: " + (filter.PaymentMethod.HasValue
                                ? filter.PaymentMethod.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Processed By: " + (filter.ProcessedByEmployeeId.HasValue
                                ? filter.ProcessedByEmployeeId.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Payment Count: " + report.PaymentCount);

                        column.Item().Text("Total Amount Paid: " + report.TotalAmountPaidDisplay);

                        column.Item().Text("Average Payment Amount: " + report.AveragePaymentAmountDisplay);

                        column.Item().PaddingTop(15).Text("Payment Method Summary").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Method").FontSize(8).Bold();
                                header.Cell().Text("Payments").FontSize(8).Bold();
                                header.Cell().Text("Total").FontSize(8).Bold();
                                header.Cell().Text("Voided").FontSize(8).Bold();
                                header.Cell().Text("Voided Amount").FontSize(8).Bold();
                                header.Cell().Text("Net").FontSize(8).Bold();
                            });

                            foreach (PaymentMethodSummaryRowVM row in report.PaymentMethodSummaryRows)
                            {
                                table.Cell().Text(row.PaymentMethodDisplay).FontSize(8);
                                table.Cell().Text(row.PaymentCount.ToString()).FontSize(8);
                                table.Cell().Text(row.TotalPaymentAmountDisplay).FontSize(8);
                                table.Cell().Text(row.VoidedPaymentCount.ToString()).FontSize(8);
                                table.Cell().Text(row.VoidedPaymentAmountDisplay).FontSize(8);
                                table.Cell().Text(row.NetPaymentAmountDisplay).FontSize(8);
                            }
                        });

                        column.Item().PaddingTop(15).Text("Payments").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.6f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.8f);
                                columns.RelativeColumn(1.6f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Invoice").FontSize(8).Bold();
                                header.Cell().Text("Type").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Date").FontSize(8).Bold();
                                header.Cell().Text("Amount").FontSize(8).Bold();
                                header.Cell().Text("Method").FontSize(8).Bold();
                                header.Cell().Text("Reference").FontSize(8).Bold();
                                header.Cell().Text("Processed By").FontSize(8).Bold();
                            });

                            foreach (PaymentReportRowVM row in report.PaymentReportRows)
                            {
                                table.Cell().Text(row.InvoiceId.ToString()).FontSize(8);
                                table.Cell().Text(row.InvoiceTypeDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.PaymentDateDisplay).FontSize(8);
                                table.Cell().Text(row.AmountPaidDisplay).FontSize(8);
                                table.Cell().Text(row.PaymentMethodDisplay).FontSize(8);
                                table.Cell().Text(row.TransactionReferenceDisplay).FontSize(8);
                                table.Cell().Text(row.ProcessedByEmployeeNameDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GeneratePetCareReportPdf(PetCareReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Pet Care Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        PetCareReportFilterVM filter = report.PetCareReportFilter;

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet Name: " + (!string.IsNullOrWhiteSpace(filter.PetName)
                                ? filter.PetName
                                : "All"));

                        column.Item().Text(
                            "Species: " + (filter.Species.HasValue
                                ? filter.Species.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Has Diet: " + (filter.HasDiet.HasValue
                                ? (filter.HasDiet.Value ? "Yes" : "No")
                                : "All"));

                        column.Item().Text(
                            "Has Medication: " + (filter.HasMedication.HasValue
                                ? (filter.HasMedication.Value ? "Yes" : "No")
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Total Pets: " + report.TotalCount);

                        column.Item().Text("Pets With Diets: " + report.DietCount);

                        column.Item().Text("Pets With Medications: " + report.MedicationCount);

                        column.Item().PaddingTop(15).Text("Pet Care").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(0.8f);
                                columns.RelativeColumn(1.7f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(2.2f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(0.8f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(2.0f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Pet").FontSize(7).Bold();
                                header.Cell().Text("Customer").FontSize(7).Bold();
                                header.Cell().Text("Species").FontSize(7).Bold();
                                header.Cell().Text("Diet").FontSize(7).Bold();
                                header.Cell().Text("Amount").FontSize(7).Bold();
                                header.Cell().Text("Frequency").FontSize(7).Bold();
                                header.Cell().Text("Diet Notes").FontSize(7).Bold();
                                header.Cell().Text("Medication").FontSize(7).Bold();
                                header.Cell().Text("Dosage").FontSize(7).Bold();
                                header.Cell().Text("Route").FontSize(7).Bold();
                                header.Cell().Text("Frequency").FontSize(7).Bold();
                                header.Cell().Text("Start").FontSize(7).Bold();
                                header.Cell().Text("End").FontSize(7).Bold();
                                header.Cell().Text("Med Notes").FontSize(7).Bold();
                            });

                            foreach (PetCareReportRowVM row in report.PetCareReportRows)
                            {
                                table.Cell().Text(row.PetNameDisplay).FontSize(7);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(7);
                                table.Cell().Text(row.SpeciesDisplay).FontSize(7);
                                table.Cell().Text(row.DietNameDisplay).FontSize(7);
                                table.Cell().Text(row.FeedingAmountDisplay).FontSize(7);
                                table.Cell().Text(row.FeedingFrequencyDisplay).FontSize(7);
                                table.Cell().Text(row.DietNotesDisplay).FontSize(7);
                                table.Cell().Text(row.MedicationNameDisplay).FontSize(7);
                                table.Cell().Text(row.DosageDisplay).FontSize(7);
                                table.Cell().Text(row.MedicationRouteDisplay).FontSize(7);
                                table.Cell().Text(row.MedicationFrequencyDisplay).FontSize(7);
                                table.Cell().Text(row.MedicationStartDateDisplay).FontSize(7);
                                table.Cell().Text(row.MedicationEndDateDisplay).FontSize(7);
                                table.Cell().Text(row.MedicationNotesDisplay).FontSize(7);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }


        public byte[] GenerateRevenueReportPdf(RevenueReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Revenue Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        RevenueReportFilterVM filter = report.RevenueReportFilter;

                        column.Item().Text(
                            "Invoice Start Date: " + filter.InvoiceStartDate.ToString("MM/dd/yyyy"));

                        column.Item().Text(
                            "Invoice End Date: " + filter.InvoiceEndDate.ToString("MM/dd/yyyy"));

                        column.Item().Text(
                            "Invoice Type: " + (filter.InvoiceType.HasValue
                                ? filter.InvoiceType.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Invoice Status: " + (filter.InvoiceStatus.HasValue
                                ? filter.InvoiceStatus.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Total Invoices: " + report.TotalInvoiceCount);

                        column.Item().Text("Total Invoiced: " + report.TotalInvoicedDisplay);

                        column.Item().Text("Total Received: " + report.TotalReceivedDisplay);

                        column.Item().Text("Total Outstanding: " + report.TotalOutstandingDisplay);

                        column.Item().Text("Average Invoice Value: " + report.AverageInvoiceValueDisplay);

                        column.Item().Text("Average Amount Received: " + report.AverageAmountReceivedDisplay);

                        column.Item().Text("Total Voided Invoices: " + report.TotalVoidedInvoiceCount);

                        column.Item().Text("Total Voided Amount: " + report.TotalVoidedAmountDisplay);

                        column.Item().PaddingTop(15).Text("Revenue Details").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Type").FontSize(8).Bold();
                                header.Cell().Text("Date").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Subtotal").FontSize(8).Bold();
                                header.Cell().Text("Tax").FontSize(8).Bold();
                                header.Cell().Text("Discount").FontSize(8).Bold();
                                header.Cell().Text("Total").FontSize(8).Bold();
                                header.Cell().Text("Paid").FontSize(8).Bold();
                                header.Cell().Text("Balance").FontSize(8).Bold();
                            });

                            foreach (RevenueReportRowVM row in report.RevenueReportRows)
                            {
                                table.Cell().Text(row.InvoiceTypeDisplay).FontSize(8);
                                table.Cell().Text(row.InvoiceDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.StatusDisplay).FontSize(8);
                                table.Cell().Text(row.SubtotalDisplay).FontSize(8);
                                table.Cell().Text(row.TaxAmountDisplay).FontSize(8);
                                table.Cell().Text(row.DiscountAmountDisplay).FontSize(8);
                                table.Cell().Text(row.TotalAmountDisplay).FontSize(8);
                                table.Cell().Text(row.AmountPaidDisplay).FontSize(8);
                                table.Cell().Text(row.BalanceDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();

        }


        public byte[] GenerateVoidedTransactionsReportPdf(VoidedTransactionsReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Voided Transactions Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        VoidedTransactionsReportFilterVM filter = report.VoidedTransactionsReportFilter;

                        column.Item().Text(
                            "Voided Start Date: " + filter.VoidedStartDate.ToString("MM/dd/yyyy"));

                        column.Item().Text(
                            "Voided End Date: " + filter.VoidedEndDate.ToString("MM/dd/yyyy"));

                        column.Item().Text(
                            "Invoice Type: " + (filter.InvoiceType.HasValue
                                ? filter.InvoiceType.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Payment Method: " + (filter.PaymentMethod.HasValue
                                ? filter.PaymentMethod.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Voided By: " + (filter.VoidedByEmployeeId.HasValue
                                ? filter.VoidedByEmployeeId.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Voided Invoice Count: " + report.VoidedInvoiceCount);

                        column.Item().Text("Voided Payment Count: " + report.VoidedPaymentCount);

                        column.Item().Text("Total Invoice Amount Voided: " + report.TotalInvoiceAmountVoidedDisplay);

                        column.Item().Text("Total Payment Amount Voided: " + report.TotalPaymentAmountVoidedDisplay);

                        column.Item().PaddingTop(15).Text("Voided Transactions").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.6f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Type").FontSize(8).Bold();
                                header.Cell().Text("Invoice Type").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Original Date").FontSize(8).Bold();
                                header.Cell().Text("Amount").FontSize(8).Bold();
                                header.Cell().Text("Method").FontSize(8).Bold();
                                header.Cell().Text("Reference").FontSize(8).Bold();
                                header.Cell().Text("Processed By").FontSize(8).Bold();
                                header.Cell().Text("Voided Date").FontSize(8).Bold();
                                header.Cell().Text("Voided By").FontSize(8).Bold();
                                header.Cell().Text("Reason").FontSize(8).Bold();
                            });

                            foreach (VoidedTransactionsReportRowVM row in report.VoidedTransactionsReportRows)
                            {
                                table.Cell().Text(row.TransactionTypeDisplay).FontSize(8);
                                table.Cell().Text(row.InvoiceTypeDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.OriginalTransactionDateDisplay).FontSize(8);
                                table.Cell().Text(row.AmountVoidedDisplay).FontSize(8);
                                table.Cell().Text(row.PaymentMethodDisplay).FontSize(8);
                                table.Cell().Text(row.TransactionReferenceDisplay).FontSize(8);
                                table.Cell().Text(row.ProcessedByEmployeeNameDisplay).FontSize(8);
                                table.Cell().Text(row.VoidedDateDisplay).FontSize(8);
                                table.Cell().Text(row.VoidedByEmployeeNameDisplay).FontSize(8);
                                table.Cell().Text(row.VoidedReasonDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }


        public byte[] GenerateCurrentBoardersReportPdf(CurrentBoardersReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Current Boarders Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        CurrentBoardersReportFilterVM filter = report.CurrentBoardersReportFilter;

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Boarding Unit: " + (filter.BoardingUnitId.HasValue
                                ? filter.BoardingUnitId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Species: " + (filter.Species.HasValue
                                ? filter.Species.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Total Current Boarders: " + report.TotalCurrentBoarderCount);

                        column.Item().PaddingTop(15).Text("Current Boarders").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(0.8f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.7f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.8f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Unit").FontSize(8).Bold();
                                header.Cell().Text("Species").FontSize(8).Bold();
                                header.Cell().Text("Check In").FontSize(8).Bold();
                                header.Cell().Text("Scheduled Out").FontSize(8).Bold();
                                header.Cell().Text("Stay").FontSize(8).Bold();
                                header.Cell().Text("Diet").FontSize(8).Bold();
                                header.Cell().Text("Medication").FontSize(8).Bold();
                                header.Cell().Text("Vaccine").FontSize(8).Bold();
                                header.Cell().Text("Notes").FontSize(8).Bold();
                            });

                            foreach (CurrentBoardersReportRowVM row in report.CurrentBoardersReportRows)
                            {
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.BoardingUnitDisplay).FontSize(8);
                                table.Cell().Text(row.SpeciesDisplay).FontSize(8);
                                table.Cell().Text(row.CheckInDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.ScheduledCheckOutDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.LengthOfStayDisplay).FontSize(8);
                                table.Cell().Text(row.DietDisplay).FontSize(8);
                                table.Cell().Text(row.MedicationStatusDisplay).FontSize(8);
                                table.Cell().Text(row.VaccineComplianceDisplay).FontSize(8);
                                table.Cell().Text(row.NotesDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }

        
        public byte[] GenerateOutstandingBalanceReportPdf(OutstandingBalanceReportVM report)
        { 
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Outstanding Balance Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        OutstandingBalanceReportFilterVM filter = report.OutstandingBalanceReportFilter;

                        column.Item().Text(
                            "Invoice Start Date: " + (filter.InvoiceStartDate.HasValue
                                ? filter.InvoiceStartDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Invoice End Date: " + (filter.InvoiceEndDate.HasValue
                                ? filter.InvoiceEndDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Minimum Balance: " + (filter.MinimumBalance.HasValue
                                ? filter.MinimumBalance.Value.ToString("C")
                                : "All"));

                        column.Item().Text(
                            "Invoice Status: " + (filter.InvoiceStatus.HasValue
                                ? filter.InvoiceStatus.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Invoice Count: " + report.InvoiceCount);

                        column.Item().Text("Total Outstanding Balance: " + report.TotalOutstandingBalanceDisplay);

                        column.Item().PaddingTop(15).Text("Outstanding Balances").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.4f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Invoice Date").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Total").FontSize(8).Bold();
                                header.Cell().Text("Paid").FontSize(8).Bold();
                                header.Cell().Text("Last Payment").FontSize(8).Bold();
                                header.Cell().Text("Outstanding").FontSize(8).Bold();
                            });

                            foreach (OutstandingBalanceReportRowVM row in report.OutstandingBalanceReportRows)
                            {
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.InvoiceDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.InvoiceStatusDisplay).FontSize(8);
                                table.Cell().Text(row.TotalAmountDisplay).FontSize(8);
                                table.Cell().Text(row.AmountPaidDisplay).FontSize(8);
                                table.Cell().Text(row.LastPaymentDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.OutstandingBalanceDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();

        }

        
        public byte[] GenerateInvoiceReportPdf(InvoiceReportVM report)
        { 
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Invoice Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        InvoiceReportFilterVM filter = report.InvoiceReportFilter;

                        column.Item().Text(
                            "Invoice Start Date: " + (filter.InvoiceStartDate.HasValue
                                ? filter.InvoiceStartDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Invoice End Date: " + (filter.InvoiceEndDate.HasValue
                                ? filter.InvoiceEndDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Invoice Type: " + (filter.InvoiceType.HasValue
                                ? filter.InvoiceType.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Invoice Status: " + (filter.InvoiceStatus.HasValue
                                ? filter.InvoiceStatus.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Invoice Count: " + report.InvoiceCount);

                        column.Item().Text("Total Subtotal: " + report.TotalSubtotalDisplay);

                        column.Item().Text("Total Tax: " + report.TotalTaxDisplay);

                        column.Item().Text("Total Discount: " + report.TotalDiscountDisplay);

                        column.Item().Text("Total Amount: " + report.TotalAmountDisplay);

                        column.Item().Text("Total Amount Paid: " + report.TotalAmountPaidDisplay);

                        column.Item().Text("Total Balance: " + report.TotalBalanceDisplay);

                        column.Item().PaddingTop(15).Text("Invoices").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(0.8f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.0f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Type").FontSize(8).Bold();
                                header.Cell().Text("Items").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Boarding").FontSize(8).Bold();
                                header.Cell().Text("Date").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Subtotal").FontSize(8).Bold();
                                header.Cell().Text("Tax").FontSize(8).Bold();
                                header.Cell().Text("Discount").FontSize(8).Bold();
                                header.Cell().Text("Total").FontSize(8).Bold();
                                header.Cell().Text("Paid").FontSize(8).Bold();
                                header.Cell().Text("Balance").FontSize(8).Bold();
                            });

                            foreach (InvoiceReportRowVM row in report.InvoiceReportRows)
                            {
                                table.Cell().Text(row.InvoiceTypeDisplay).FontSize(8);
                                table.Cell().Text(row.InvoiceItemCount.ToString()).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.BoardingDisplay).FontSize(8);
                                table.Cell().Text(row.InvoiceDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.StatusDisplay).FontSize(8);
                                table.Cell().Text(row.SubtotalDisplay).FontSize(8);
                                table.Cell().Text(row.TaxAmountDisplay).FontSize(8);
                                table.Cell().Text(row.DiscountAmountDisplay).FontSize(8);
                                table.Cell().Text(row.TotalAmountDisplay).FontSize(8);
                                table.Cell().Text(row.AmountPaidDisplay).FontSize(8);
                                table.Cell().Text(row.BalanceDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }

        
        public byte[] GenerateVaccineComplianceReportPdf(VaccineComplianceReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Vaccine Compliance Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        VaccineComplianceReportFilterVM filter = report.VaccineComplianceReportFilter;

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Expiration Start Date: " + (filter.ExpirationStartDate.HasValue
                                ? filter.ExpirationStartDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Expiration End Date: " + (filter.ExpirationEndDate.HasValue
                                ? filter.ExpirationEndDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Compliance Status: " + (filter.ComplianceStatus.HasValue
                                ? filter.ComplianceStatus.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Species: " + (filter.Species.HasValue
                                ? filter.Species.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Total Records: " + report.TotalCount);

                        column.Item().Text("Expired: " + report.ExpiredCount);

                        column.Item().Text("Expiring Today: " + report.ExpiringTodayCount);

                        column.Item().Text("Expiring Tomorrow: " + report.ExpiringTomorrowCount);

                        column.Item().Text("Expiring Within 15 Days: " + report.ExpiringWithin15DaysCount);

                        column.Item().Text("Expiring Within 30 Days: " + report.ExpiringWithin30DaysCount);

                        column.Item().Text("Current: " + report.CurrentCount);

                        column.Item().Text("Missing: " + report.MissingCount);

                        column.Item().PaddingTop(15).Text("Vaccine Compliance").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.7f);
                                columns.RelativeColumn(0.6f);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.8f);
                            });

                            table.Header(header =>
                            {   
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Species").FontSize(8).Bold();
                                header.Cell().Text("Vaccine").FontSize(8).Bold();
                                header.Cell().Text("Date Given").FontSize(8).Bold();
                                header.Cell().Text("Expiration").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Days").FontSize(8).Bold();
                                header.Cell().Text("Document").FontSize(8).Bold();
                                header.Cell().Text("Notes").FontSize(8).Bold();
                            });

                            foreach (VaccineComplianceReportRowVM row in report.VaccineComplianceReportRows)
                            {
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.SpeciesDisplay).FontSize(8);
                                table.Cell().Text(row.VaccineNameDisplay).FontSize(8);
                                table.Cell().Text(row.DateGivenDisplay).FontSize(8);
                                table.Cell().Text(row.ExpirationDateDisplay).FontSize(8);
                                table.Cell().Text(row.VaccineComplianceStatusDisplay).FontSize(8);
                                table.Cell().Text(row.DaysUntilExpiration.HasValue 
                                    ? row.DaysUntilExpiration.Value.ToString()
                                    : "N/A").FontSize(8);
                                table.Cell().Text(row.DocumentFilePathDisplay).FontSize(8);
                                table.Cell().Text(row.NotesDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }

        
        public byte[] GenerateBoardingOccupancyReportPdf(BoardingOccupancyReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Boarding Occupancy Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        BoardingOccupancyReportFilterVM filter = report.BoardingOccupancyReportFilter;

                        column.Item().Text(
                            "Report Date: " + (filter.ReportDate.HasValue
                                ? filter.ReportDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Unit Type: " + (filter.UnitType.HasValue
                                ? filter.UnitType.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Species Allowed: " + (filter.SpeciesAllowed.HasValue
                                ? filter.SpeciesAllowed.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Size Category: " + (filter.SizeCategory.HasValue
                                ? filter.SizeCategory.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Total Units: " + report.TotalUnitCount);

                        column.Item().Text("Occupied Units: " + report.OccupiedUnitCount);

                        column.Item().Text("Available Units: " + report.AvailableUnitCount);

                        column.Item().Text("Occupancy Percentage: " + report.OccupancyPercentage.ToString("0.00") + "%");

                        column.Item().PaddingTop(15).Text("Boarding Occupancy").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Unit").FontSize(8).Bold();
                                header.Cell().Text("Number").FontSize(8).Bold();
                                header.Cell().Text("Type").FontSize(8).Bold();
                                header.Cell().Text("Species").FontSize(8).Bold();
                                header.Cell().Text("Size").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Check In").FontSize(8).Bold();
                                header.Cell().Text("Check Out").FontSize(8).Bold();
                            });

                            foreach (BoardingOccupancyReportRowVM row in report.BoardingOccupancyReportRows)
                            {
                                table.Cell().Text(row.UnitNameDisplay).FontSize(8);
                                table.Cell().Text(row.UnitNumberDisplay).FontSize(8);
                                table.Cell().Text(row.UnitTypeDisplay).FontSize(8);
                                table.Cell().Text(row.SpeciesAllowedDisplay).FontSize(8);
                                table.Cell().Text(row.SizeCategoryDisplay).FontSize(8);
                                table.Cell().Text(row.OccupancyStatusDisplay).FontSize(8);
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.CheckInDateDisplay).FontSize(8);
                                table.Cell().Text(row.CheckOutDateDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();
        }

        
        public byte[] GenerateDailyBoardingReportPdf(DailyBoardingReportVM report)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter.Landscape());

                    page.Margin(30);

                    page.Header().Text("Daily Boarding Report").FontSize(20).Bold();

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Item()
                            .Text("Generated: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                        column.Item().PaddingTop(15).Text("Filters").FontSize(16).Bold();

                        DailyBoardingReportFilterVM filter = report.DailyBoardingReportFilter;

                        column.Item().Text(
                            "Start Date: " + (filter.StartDate.HasValue
                                ? filter.StartDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "End Date: " + (filter.EndDate.HasValue
                                ? filter.EndDate.Value.ToString("MM/dd/yyyy")
                                : "All"));

                        column.Item().Text(
                            "Customer: " + (filter.CustomerId.HasValue
                                ? filter.CustomerId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Pet: " + (filter.PetId.HasValue
                                ? filter.PetId.Value.ToString()
                                : "All"));

                        column.Item().Text(
                            "Boarding Status: " + (filter.BoardingStatus.HasValue
                                ? filter.BoardingStatus.Value.ToString()
                                : "All"));

                        column.Item().PaddingTop(15).Text("Summary").FontSize(16).Bold();

                        column.Item().Text("Scheduled Arrivals: " + report.ScheduledArrivalCount);

                        column.Item().Text("Actual Arrivals: " + report.ActualArrivalCount);

                        column.Item().Text("Scheduled Departures: " + report.ScheduledDepartureCount);

                        column.Item().Text("Actual Departures: " + report.ActualDepartureCount);

                        column.Item().PaddingTop(15).Text("Daily Boarding").FontSize(16).Bold();

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(1.3f);
                                columns.RelativeColumn(1.6f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.0f);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(0.9f);
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Pet").FontSize(8).Bold();
                                header.Cell().Text("Customer").FontSize(8).Bold();
                                header.Cell().Text("Unit").FontSize(8).Bold();
                                header.Cell().Text("Start").FontSize(8).Bold();
                                header.Cell().Text("End").FontSize(8).Bold();
                                header.Cell().Text("Check In").FontSize(8).Bold();
                                header.Cell().Text("Check Out").FontSize(8).Bold();
                                header.Cell().Text("Status").FontSize(8).Bold();
                                header.Cell().Text("Vaccine").FontSize(8).Bold();
                                header.Cell().Text("Balance").FontSize(8).Bold();
                                header.Cell().Text("Notes").FontSize(8).Bold();
                            });

                            foreach (DailyBoardingReportRowVM row in report.DailyBoardingReportRows)
                            {
                                table.Cell().Text(row.PetNameDisplay).FontSize(8);
                                table.Cell().Text(row.CustomerNameDisplay).FontSize(8);
                                table.Cell().Text(row.BoardingUnitDisplay).FontSize(8);
                                table.Cell().Text(row.StartDateDisplay).FontSize(8);
                                table.Cell().Text(row.EndDateDisplay).FontSize(8);
                                table.Cell().Text(row.CheckInDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.CheckOutDateTimeDisplay).FontSize(8);
                                table.Cell().Text(row.BoardingStatusDisplay).FontSize(8);
                                table.Cell().Text(row.VaccineComplianceDisplay).FontSize(8);
                                table.Cell().Text(row.BalanceDisplay).FontSize(8);
                                table.Cell().Text(row.NotesDisplay).FontSize(8);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");

                        text.CurrentPageNumber();

                    });
                });
            });

            return document.GeneratePdf();

        }
    }
}