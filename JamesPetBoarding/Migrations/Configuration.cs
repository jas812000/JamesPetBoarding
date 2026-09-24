namespace JamesPetBoarding.Migrations
{
    using JamesPetBoarding.Data;
    using JamesPetBoarding.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration
        : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        // Rebuilds the repeatable development/testing baseline whenever
        // Update-Database runs. DatabaseSeeder includes the initial
        // System Administrator employee.
        protected override void Seed(ApplicationDbContext context)
        {
            DatabaseSeeder.Seed(context);
        }
    }
}
