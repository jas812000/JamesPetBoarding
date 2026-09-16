# Paws & Reservations

Paws & Reservations is a full-stack pet boarding management application built with ASP.NET MVC 5, C#, Entity Framework 6, and SQL Server. The system provides staff with a centralized platform for managing customers, pets, boarding reservations, pet care information, invoicing, payments, employees, and operational and financial reporting.

The application supports the complete boarding workflow—from reservation and check-in through checkout, invoicing, and payment—while enforcing business rules, maintaining transaction and employee audit information, and providing secured account and reporting workflows.

## Overview

Paws & Reservations was designed around the day-to-day operations of a pet boarding business. Rather than treating boarding, customer records, billing, and reporting as isolated features, the application connects them into a single workflow.

Staff can maintain customer and pet information, manage boarding-unit reservations, record pet care requirements, process boarding status changes, create invoices, accept payments, and generate reports from the same application.

The system also incorporates employee-based account registration, email confirmation, password recovery, account lockout, authorization controls, financial validation, and auditable transaction workflows.

## Key Features

### Customer and Pet Management

- Maintain customer records and emergency contacts
- Manage pets and their associated customer relationships
- Store veterinarian information
- Track pet diets and medications
- Maintain vaccination and vaccine-compliance information
- Support active and inactive records without permanently deleting historical information

### Boarding Management

- Create and manage boarding reservations
- Assign boarding units
- Validate customer and pet relationships
- Detect overlapping reservations for the same boarding unit
- Track scheduled and confirmed boardings
- Record actual check-in and checkout information
- Process cancellations and no-shows
- Record employees responsible for boarding lifecycle actions

### Billing and Payments

- Create invoices for customers and pets
- Associate invoices with boardings when applicable
- Add boarding and service charges through invoice items
- Maintain invoice subtotal, total, amount paid, and outstanding balance
- Prevent payments against voided invoices
- Prevent zero, negative, and excessive payments
- Generate transaction references for payments
- Record the employee responsible for processing a payment
- Void invoices and payments while retaining audit information
- Reverse the financial effect of voided payments without deleting transaction history

### Employee and Account Management

- Maintain employee records and operational roles
- Restrict self-registration to existing active employees
- Require unique account email addresses
- Require email confirmation before normal account access
- Support password recovery through emailed reset tokens
- Apply account lockout after repeated failed login attempts
- Route authenticated employees to role-appropriate dashboards

### Reporting

The application provides operational and financial reporting across the boarding business, including customer activity, pet information, pet care, boarding activity, invoices, payments, revenue, outstanding balances, voided transactions, vaccine compliance, and related operational information.

Selected financial reports are restricted to authorized management roles.

Reports can also be exported as formatted PDF documents using QuestPDF.

---

## Application Workflow

A typical boarding transaction moves through several related areas of the system:

```text
Customer
   ↓
Pet
   ↓
Boarding Reservation
   ↓
Boarding Unit Assignment
   ↓
Check-In
   ↓
Pet Care / Boarding Services
   ↓
Check-Out
   ↓
Invoice
   ↓
Invoice Items
   ↓
Payment
   ↓
Receipt / Reporting
```

This workflow allows customer, pet, boarding, billing, and payment information to remain connected throughout the transaction.

---

## Architecture

Paws & Reservations follows the ASP.NET MVC pattern and separates application responsibilities across models, views, controllers, ViewModels, and supporting services.

```text
Browser
   ↓
ASP.NET MVC Controllers
   ↓
ViewModels / Business Workflow
   ↓
Entity Framework 6
   ↓
ApplicationDbContext
   ↓
SQL Server / LocalDB
```

### Models

Domain models represent the primary business entities, including customers, emergency contacts, pets, customer/pet relationships, veterinarians, diets, medications, vaccines, pet vaccinations, boarding units, boardings, services, invoices, invoice items, payments, employees, and contact submissions.

ASP.NET Identity data and application data share the application's Entity Framework database context.

### Controllers

MVC controllers coordinate application workflows, validation, authorization, persistence, and navigation. Dedicated controllers handle major application areas such as customers, pets, employees, boardings, invoices, invoice items, payments, reports, account management, and public-facing pages.

### ViewModels

ViewModels separate UI-specific data from persistence models and support search and filtering, create and update operations, deactivation and reactivation, boarding lifecycle actions, invoice and payment processing, reporting, and dashboard presentation.

### Views

Razor views provide the staff-facing and public-facing user interfaces while keeping database access and application workflow logic outside the presentation layer.

### Services

Supporting services handle functionality that does not belong directly in MVC controllers. PDF report generation is implemented through a dedicated `PdfReportService`.

---

## Technology Stack

| Area | Technology |
| --- | --- |
| Language | C# |
| Web Framework | ASP.NET MVC 5 |
| Runtime | .NET Framework 4.8.1 |
| Data Access | Entity Framework 6 |
| Database | SQL Server / SQL Server LocalDB |
| Authentication | ASP.NET Identity |
| Authentication Middleware | OWIN |
| UI | Razor, HTML, CSS, JavaScript |
| PDF Reporting | QuestPDF |
| Email | SMTP |
| Development Environment | Visual Studio |
| Source Control | Git / GitHub |

---

## Boarding Workflow and Business Rules

Boarding management is implemented as a workflow rather than simple record editing.

### Reservation Validation

When a boarding reservation is created or updated, the application validates:

- The customer exists and is active
- The pet exists and is active
- The boarding unit exists and is active
- The selected pet belongs to the selected customer
- Checkout occurs after check-in
- The selected boarding unit does not have a conflicting reservation

Cancelled and no-show boardings do not block the unit as active reservation conflicts.

### Boarding Status Actions

Eligible scheduled or confirmed boardings can be checked in. The application records the actual check-in time and employee responsible for the action.

Only checked-in boardings can be checked out. The actual checkout must occur after check-in, and the responsible employee is recorded.

Eligible boardings can be cancelled while preserving the date, employee, reason, and related notes. Scheduled or confirmed reservations can also be marked as no-shows while preserving audit information.

These workflow actions preserve boarding history rather than deleting records when a boarding does not proceed normally.

---

## Invoicing

Invoices connect customer, pet, boarding, service, and payment information. New invoices begin with financial values initialized to zero and can receive individual invoice items.

When an invoice item is created:

```text
Line Total = Quantity × Unit Price
```

The invoice is recalculated using:

```text
Total = Subtotal + Tax - Discount
Balance = Total - Amount Paid
```

When an invoice item is updated, the application calculates the difference between the previous and new line totals. When an item is deleted, its line total is removed from the invoice totals.

### Invoice Voiding

Invoices are not simply deleted when they need to be invalidated. The void workflow preserves the void status, reason, notes, date and time, and employee responsible for the action. Invoice items cannot be added to a voided invoice.

---

## Payment Processing

Before accepting a payment, the application verifies that:

- The invoice has not been voided
- The invoice has an outstanding balance
- The payment amount is greater than zero
- The payment does not exceed the remaining balance

Each payment receives a generated transaction reference and records the employee who processed it.

```text
Amount Paid = Previous Amount Paid + Payment
Balance = Invoice Total - Amount Paid
```

### Payment Voiding

Voiding a payment retains the original transaction and reverses its financial effect on the associated invoice:

```text
Amount Paid = Previous Amount Paid - Voided Payment
Balance = Invoice Total - Amount Paid
```

This preserves transaction history while keeping invoice financial values accurate.

---

## Authentication and Account Security

Paws & Reservations uses ASP.NET Identity and OWIN cookie authentication.

### Employee-Based Registration

An employee must already have an active employee record before an ASP.NET Identity account can be created. Registration also requires a unique email address.

### Email Confirmation and Password Recovery

Newly registered users receive an email-confirmation token. Account access requires the email address to be confirmed. Forgot-password requests use ASP.NET Identity reset tokens delivered through email.

### Password Requirements

Passwords require:

- A minimum length of six characters
- An uppercase character
- A lowercase character
- A numeric character
- A non-alphanumeric character

### Account Lockout

After five failed access attempts, an account is locked for five minutes.

### Authentication Session

Authentication uses an application cookie with a 15-minute expiration period and sliding expiration. ASP.NET Identity security-stamp validation periodically revalidates authenticated identities.

### SMTP Credentials

SMTP connection settings are read from application configuration. The SMTP password is kept out of source control and retrieved from:

```text
JamesPetBoarding_SMTP_Password
```

---

## Authorization

The application uses authenticated employee records and employee roles to control access to workflows. Examples include role-aware dashboard routing, protected boarding-management workflows, management access to financial reporting, and employee validation before authenticated workflows are performed.

Authorization is enforced server-side rather than relying solely on navigation visibility.

---

## Reporting and PDF Export

Report areas include:

- Customer activity
- Pets and pet care
- Species
- Boarding activity
- Current boarders
- Boarding occupancy
- Daily boarding activity
- Invoices
- Payments
- Revenue
- Outstanding balances
- Voided transactions
- Vaccine compliance

Reports support subject-appropriate filters, and selected financial reports are restricted to authorized management users.

QuestPDF is used to generate server-side PDF reports with titles, timestamps, selected filters, summary statistics, detailed tables, financial totals, pagination, and landscape layouts where appropriate.

PDF generation is implemented in a dedicated report service rather than embedded directly in Razor views.

---

## Data Persistence

Paws & Reservations uses Entity Framework 6 with SQL Server. The application database is configured through the `DefaultConnection` connection string.

The default local development configuration uses SQL Server LocalDB:

```text
Data Source=(LocalDb)\MSSQLLocalDB
```

Entity Framework Code First migrations maintain database schema changes. Automatic migrations are disabled so schema changes are explicitly represented through migration files.

---

## Project Structure

```text
JamesPetBoarding/
│
├── JamesPetBoarding.sln
├── README.md
├── LICENSE.md
│
├── docs/
│   ├── README.md
│   └── wireframes/
│       └── README.md
│
├── JamesPetBoarding/
│   ├── App_Start/
│   ├── Controllers/
│   ├── Enums/
│   ├── Migrations/
│   ├── Models/
│   ├── Services/
│   ├── ViewModels/
│   ├── Views/
│   ├── Web.config
│   └── JamesPetBoarding.csproj
│
└── JamesPetBoardingTests/
```

| Directory | Purpose |
| --- | --- |
| `Controllers` | MVC request handling and application workflows |
| `Models` | Entity Framework domain and persistence models |
| `ViewModels` | UI and workflow-specific data models |
| `Views` | Razor user interfaces |
| `Enums` | Domain-specific enumerations |
| `Migrations` | Entity Framework database migrations |
| `Services` | Supporting services such as PDF generation |
| `App_Start` | Identity, authentication, routing, and application configuration |

---

# Getting Started

## Prerequisites

Before building the application, install:

- Windows
- Visual Studio with ASP.NET and web development support
- .NET Framework 4.8.1 developer tools
- SQL Server LocalDB
- Git

A supported SQL Server instance can be used instead of LocalDB by changing the connection string.

## Clone the Repository

```bash
git clone https://github.com/jas812000/JamesPetBoarding.git
cd JamesPetBoarding
```

Open `JamesPetBoarding.sln` in Visual Studio and restore NuGet dependencies if Visual Studio does not restore them automatically.

## Database Setup

Verify the `DefaultConnection` connection string in `Web.config`.

From Visual Studio, open **Tools → NuGet Package Manager → Package Manager Console**, select the `JamesPetBoarding` project as the default project, and run:

```powershell
Update-Database
```

This creates or updates the development database using the migrations stored in `Migrations`.

## Initial Administrator Bootstrap

The registration workflow requires an existing active employee record before an Identity account can be registered. A fresh database therefore requires an initial administrative employee record to bootstrap normal registration.

Before using the application in another environment:

1. Configure the bootstrap employee with a valid development email address that can receive confirmation email.
2. Apply the database migrations.
3. Start the application.
4. Register using the same email address as the active bootstrap employee.
5. Confirm the account through email.
6. Verify administrative access.
7. Remove or disable temporary bootstrap behavior once permanent administrator access has been established.

A temporary bootstrap administrator should not remain as an automatically recreated permanent account.

## Email Configuration

Email is used for account confirmation and password recovery. SMTP settings are stored in `Web.config`:

```text
SmtpHost
SmtpPort
SmtpEnableSsl
SmtpUsername
SmtpFromAddress
```

The SMTP password is not stored in the repository. Set it through the Windows environment variable:

```text
JamesPetBoarding_SMTP_Password
```

The current development configuration uses Gmail SMTP. Use an appropriate Gmail App Password rather than storing account credentials in source code.

Restart Visual Studio after changing environment variables.

## Build the Application

1. Open `JamesPetBoarding.sln`.
2. Select the desired build configuration.
3. Choose **Build → Build Solution**.

## Run the Application

With `JamesPetBoarding` configured as the startup project, run the application from Visual Studio using IIS Express.

For a fresh environment:

1. Apply the Entity Framework migrations.
2. Configure the bootstrap administrator.
3. Configure SMTP and the SMTP password environment variable.
4. Start the application.
5. Register the bootstrap employee.
6. Confirm the registration email.
7. Sign in.

---

## Testing

The repository currently contains an NUnit test project and test infrastructure, but meaningful automated application test coverage has not yet been implemented.

The existing test project should not be interpreted as evidence of production test coverage.

---

## Configuration and Secrets

Sensitive credentials should not be committed to source control. Review environment-specific values before running the application outside the original development environment, particularly:

- Database connection strings
- SMTP host and port
- SMTP username
- SMTP sender address
- SMTP password environment variable
- Bootstrap administrator information

---

## Documentation

Supporting documentation will be maintained under the repository's `docs` directory.

Planned supporting documentation includes:

- Application screenshots
- Wireframes
- Architecture diagrams
- Database and domain diagrams
- Workflow documentation

---

## Project Status

**Application development: Complete**

The primary application workflows are implemented, including customer and pet management, boarding operations, pet care records, employee workflows, authentication and account recovery, invoicing, payment processing, operational and financial reporting, and PDF export.

Current work is focused on repository documentation and portfolio presentation rather than adding core application functionality.

---

## Future Improvements

Potential improvements that do not affect the completed core application include:

- Add meaningful automated unit and integration test coverage
- Add technical and architectural documentation
- Add portfolio-quality application screenshots
- Add workflow and architecture diagrams
- Improve deployment portability beyond the current Windows/.NET Framework development environment

---

## License

Copyright © James Stevens.

See the repository's [`LICENSE.md`](LICENSE.md) file for permitted uses of the source code.
