# Development Database Seeding

The EF6 migration `Seed` method creates a repeatable development baseline for testing CRUD workflows, boarding operations, billing, payments, voids, and reports.

## Normal clean reset

Use this when application data has been changed during testing and you want to restore the standard baseline without deleting login accounts.

1. Stop the application before resetting data.
2. In SSMS, connect to `(localdb)\MSSQLLocalDB` and select `JamesPetBoarding`.
3. Open `ResetApplicationData.sql`.
4. Confirm that the selected database shown in SSMS is `JamesPetBoarding`.
5. Change `DECLARE @ConfirmReset bit = 0;` to `DECLARE @ConfirmReset bit = 1;`.
6. Execute the script.
7. In Visual Studio Package Manager Console, select `JamesPetBoarding` as the Default project.
8. Run `Update-Database -Verbose`.
9. Execute `VerifySeedData.sql` in SSMS.

The normal reset deletes all application-created and manually added business records. It preserves the ASP.NET Identity tables, existing login accounts, `dbo.__MigrationHistory`, and the database schema.

## Full database deletion and rebuild

Use this only when the entire LocalDB database—including login accountsâ€”should be discarded.

Back up anything that must be retained, stop IIS Express, connect to `(localdb)\MSSQLLocalDB`, open a query against `master`, and run:

```sql
USE master;

IF DB_ID(N'JamesPetBoarding') IS NOT NULL
BEGIN
    ALTER DATABASE [JamesPetBoarding]
        SET SINGLE_USER
        WITH ROLLBACK IMMEDIATE;

    DROP DATABASE [JamesPetBoarding];
END;
```

Then run `Update-Database -Verbose` from Visual Studio Package Manager Console. EF6 will recreate the database schema and seed the development baseline. Because the Identity tables were deleted, the login account must be registered again using the email of an active seeded employee.

## Source control

The LocalDB database is stored outside the repository and is not pushed to GitHub. The seed classes, migrations, reset script, verification script, and documentation are source-controlled. Runtime records added through the application remain local unless they are deliberately added to seed code.