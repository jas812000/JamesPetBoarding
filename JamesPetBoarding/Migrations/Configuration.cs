namespace JamesPetBoarding.Migrations
{
    using JamesPetBoarding.Enums;
    using JamesPetBoarding.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JamesPetBoarding.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        // Bootstrap the initial development administrator so the first management
        // account can be registered. Replace "ADD EMAIL HERE" with the development
        // email address used for testing. The email must be valid and able to receive
        // the account confirmation email. Remove this bootstrap seed after permanent
        // administrator access has been established.
        protected override void Seed(JamesPetBoarding.Models.ApplicationDbContext context)
        {
            context.Employees.AddOrUpdate(
                x => x.Email,
                new EmployeeModel
                {
                    FirstName = "System",
                    LastName = "Administrator",
                    Role = EmployeeRoleEnum.Admin,
                    Phone = "555-555-0100",
                    Email = "james.sandbox.lab@gmail.com",
                    IsActive = true,
                    Notes = "Initial development administrator."
                });

            context.SaveChanges();
        }
    }
}
