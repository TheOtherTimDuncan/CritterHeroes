namespace CH.DatabaseMigrator.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CritterHeroes.Web.Data.Models.Identity;
    using EntityFramework.DatabaseMigrator.Migrations;

    internal sealed class Configuration : BaseMigrationConfiguration<CH.DatabaseMigrator.Migrations.MigrationsDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CH.DatabaseMigrator.Migrations.MigrationsDataContext context)
        {
            foreach (string role in UserRole.GetAll())
            {
                AppRole appRole = context.Roles.SingleOrDefault(x => x.Name == role);
                if (appRole == null)
                {
                    appRole = new AppRole()
                    {
                        Name = role
                    };
                    context.Roles.Add(appRole);

                    Logger.Verbose("Added role " + role);
                }
            }
        }
    }
}
