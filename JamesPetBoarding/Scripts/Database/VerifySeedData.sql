/* Run after Update-Database -Verbose completes successfully. */

SET NOCOUNT ON;

IF DB_NAME() <> N'JamesPetBoarding'
BEGIN
    THROW 50001, 'Verification stopped: the selected database is not JamesPetBoarding.', 1;
END;

DECLARE @Counts TABLE
(
    SortOrder int NOT NULL,
    EntityName nvarchar(100) NOT NULL,
    ExpectedCount int NOT NULL,
    ActualCount int NOT NULL
);

INSERT INTO @Counts VALUES (1,  N'Employees',           43, (SELECT COUNT(*) FROM dbo.EmployeeModels));
INSERT INTO @Counts VALUES (2,  N'Our Team members',    12, (SELECT COUNT(*) FROM dbo.OurTeamMemberModels));
INSERT INTO @Counts VALUES (3,  N'Veterinarians',        8, (SELECT COUNT(*) FROM dbo.VeterinarianModels));
INSERT INTO @Counts VALUES (4,  N'Vaccines',            15, (SELECT COUNT(*) FROM dbo.VaccineModels));
INSERT INTO @Counts VALUES (5,  N'Services',            24, (SELECT COUNT(*) FROM dbo.ServiceModels));
INSERT INTO @Counts VALUES (6,  N'Boarding units',      18, (SELECT COUNT(*) FROM dbo.BoardingUnitModels));
INSERT INTO @Counts VALUES (7,  N'Customers',           16, (SELECT COUNT(*) FROM dbo.CustomerModels));
INSERT INTO @Counts VALUES (8,  N'Emergency contacts',   8, (SELECT COUNT(*) FROM dbo.EmergencyContactModels));
INSERT INTO @Counts VALUES (9,  N'Pets',                24, (SELECT COUNT(*) FROM dbo.PetModels));
INSERT INTO @Counts VALUES (10, N'Customer-pet links',  26, (SELECT COUNT(*) FROM dbo.CustomerPetModels));
INSERT INTO @Counts VALUES (11, N'Diets',               14, (SELECT COUNT(*) FROM dbo.DietModels));
INSERT INTO @Counts VALUES (12, N'Medications',          5, (SELECT COUNT(*) FROM dbo.MedicationModels));
INSERT INTO @Counts VALUES (13, N'Pet vaccines',        57, (SELECT COUNT(*) FROM dbo.PetVaccineModels));
INSERT INTO @Counts VALUES (14, N'Boardings',           20, (SELECT COUNT(*) FROM dbo.BoardingModels));
INSERT INTO @Counts VALUES (15, N'Invoices',            14, (SELECT COUNT(*) FROM dbo.InvoiceModels));
INSERT INTO @Counts VALUES (16, N'Invoice items',       17, (SELECT COUNT(*) FROM dbo.InvoiceItemModels));
INSERT INTO @Counts VALUES (17, N'Payments',             9, (SELECT COUNT(*) FROM dbo.PaymentModels));

SELECT
    EntityName,
    ExpectedCount,
    ActualCount,
    CASE
        WHEN ExpectedCount = ActualCount THEN N'PASS'
        ELSE N'FAIL'
    END AS VerificationResult
FROM @Counts
ORDER BY SortOrder;

SELECT
    CASE WHEN EXISTS
    (
        SELECT 1
        FROM @Counts
        WHERE ExpectedCount <> ActualCount
    )
    THEN N'COUNT VERIFICATION FAILED'
    ELSE N'ALL EXPECTED COUNTS PASSED'
    END AS CountSummary;

/* Integrity checks: every returned count should be zero. */
SELECT N'Invoice arithmetic mismatches' AS IntegrityCheck, COUNT(*) AS ViolationCount
FROM dbo.InvoiceModels
WHERE TotalAmount <> Subtotal + TaxAmount - DiscountAmount

UNION ALL

SELECT N'Invoice valid-payment mismatches', COUNT(*)
FROM dbo.InvoiceModels AS invoice
WHERE invoice.AmountPaid <>
(
    SELECT COALESCE(SUM(payment.Amount), 0)
    FROM dbo.PaymentModels AS payment
    WHERE payment.InvoiceId = invoice.InvoiceId
      AND payment.IsVoided = 0
)

UNION ALL

SELECT N'Non-void invoice balance mismatches', COUNT(*)
FROM dbo.InvoiceModels
WHERE InvoiceStatus <> 8
  AND Balance <> TotalAmount - AmountPaid

UNION ALL

SELECT N'Invalid boarding date ranges', COUNT(*)
FROM dbo.BoardingModels
WHERE EndDateTime <= StartDateTime

UNION ALL

SELECT N'Pets without an owner', COUNT(*)
FROM dbo.PetModels AS pet
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.CustomerPetModels AS customerPet
    WHERE customerPet.PetId = pet.PetId
      AND customerPet.RelationshipType = 1
)

UNION ALL

SELECT N'Pet-vaccine species mismatches', COUNT(*)
FROM dbo.PetVaccineModels AS petVaccine
INNER JOIN dbo.PetModels AS pet
    ON pet.PetId = petVaccine.PetId
INNER JOIN dbo.VaccineModels AS vaccine
    ON vaccine.VaccineId = petVaccine.VaccineId
WHERE pet.Species <> vaccine.Species

UNION ALL

SELECT N'Invalid vaccine date ranges', COUNT(*)
FROM dbo.PetVaccineModels
WHERE ExpirationDate < DateGiven

UNION ALL

SELECT N'Duplicate employee emails', COUNT(*)
FROM
(
    SELECT Email
    FROM dbo.EmployeeModels
    GROUP BY Email
    HAVING COUNT(*) > 1
) AS duplicateEmployees

UNION ALL

SELECT N'Duplicate Our Team positions', COUNT(*)
FROM
(
    SELECT DisplayOrder
    FROM dbo.OurTeamMemberModels
    GROUP BY DisplayOrder
    HAVING COUNT(*) > 1
) AS duplicatePositions;

/* Useful report-distribution checks. */
SELECT Status, COUNT(*) AS BoardingCount
FROM dbo.BoardingModels
GROUP BY Status
ORDER BY Status;

SELECT InvoiceStatus, COUNT(*) AS InvoiceCount
FROM dbo.InvoiceModels
GROUP BY InvoiceStatus
ORDER BY InvoiceStatus;

SELECT
    COUNT(*) AS CurrentBoarderCount
FROM dbo.BoardingModels
WHERE Status = 3;

SELECT
    COUNT(*) AS VoidedInvoiceCount
FROM dbo.InvoiceModels
WHERE InvoiceStatus = 8;

SELECT
    COUNT(*) AS VoidedPaymentCount
FROM dbo.PaymentModels
WHERE IsVoided = 1;
