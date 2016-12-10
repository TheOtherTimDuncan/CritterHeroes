using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CH.DatabaseMigrator.Migrations;

namespace CH.DatabaseMigrator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MigrationsDataContext.SetDatabaseDirectory();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EntityFramework.DatabaseMigrator.DatabaseMigrator());
        }
    }
}
