/*
    Paws & Reservations development database reset

    Deletes application/business data while preserving:
      - ASP.NET Identity accounts and security tables
      - dbo.__MigrationHistory
      - the database schema

    After this succeeds, run Update-Database -Verbose to restore the seed baseline.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @ConfirmReset bit = 0;

IF DB_NAME() <> N'JamesPetBoarding'
BEGIN
    RAISERROR(
        'Reset stopped: the selected database is not JamesPetBoarding.',
        16,
        1);

    RETURN;
END;

IF @ConfirmReset <> 1
BEGIN
    RAISERROR(
        'Reset stopped: change @ConfirmReset to 1 and run the script again.',
        16,
        1);

    RETURN;
END;

BEGIN TRY
    BEGIN TRANSACTION;

    DELETE FROM dbo.PaymentModels;
    DELETE FROM dbo.InvoiceItemModels;
    DELETE FROM dbo.InvoiceModels;
    DELETE FROM dbo.BoardingModels;

    DELETE FROM dbo.PetVaccineModels;
    DELETE FROM dbo.MedicationModels;
    DELETE FROM dbo.DietModels;
    DELETE FROM dbo.CustomerPetModels;
    DELETE FROM dbo.EmergencyContactModels;

    DELETE FROM dbo.OurTeamMemberModels;
    DELETE FROM dbo.ContactUsSubmissionModels;

    DELETE FROM dbo.PetModels;
    DELETE FROM dbo.CustomerModels;
    DELETE FROM dbo.BoardingUnitModels;
    DELETE FROM dbo.ServiceModels;
    DELETE FROM dbo.VaccineModels;
    DELETE FROM dbo.VeterinarianModels;
    DELETE FROM dbo.EmployeeModels;

    COMMIT TRANSACTION;

    SELECT
        DB_NAME() AS DatabaseName,
        N'Application data deleted. Identity accounts, migrations, and schema were preserved.' AS Result;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    DECLARE @ErrorMessage nvarchar(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity int = ERROR_SEVERITY();
    DECLARE @ErrorState int = ERROR_STATE();

    RAISERROR(
        @ErrorMessage,
        @ErrorSeverity,
        @ErrorState);
END CATCH;
