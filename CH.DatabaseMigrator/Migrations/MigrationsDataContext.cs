using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Data.Contexts;
using CritterHeroes.Web.Data.Models;
using TOTD.Utility.UnitTestHelpers;

namespace CH.DatabaseMigrator.Migrations
{
    public class MigrationsDataContext : AppUserCommandStorageContext
    {
        public static void SetDatabaseDirectory()
        {
            string databasePath = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "Databases");
            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }
            AppDomain.CurrentDomain.SetData("DataDirectory", databasePath);
        }

        public MigrationsDataContext()
          : base("CritterHeroesMigrations", null)
        {
        }

        public virtual IDbSet<Organization> Organizations
        {
            get;
            set;
        }

        public virtual IDbSet<Species> Species
        {
            get;
            set;
        }

        public virtual IDbSet<State> States
        {
            get;
            set;
        }

        public virtual IDbSet<PhoneType> PhoneTypes
        {
            get;
            set;
        }
    }
}
