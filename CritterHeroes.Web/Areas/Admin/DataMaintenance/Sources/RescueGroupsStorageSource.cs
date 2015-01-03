using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Areas.Admin.DataMaintenance.Sources
{
    public class RescueGroupsStorageSource : IStorageSource
    {
        public int ID
        {
            get
            {
                return 1;
            }
        }

        public string Title
        {
            get
            {
                return "Rescue Groups";
            }
        }
    }
}