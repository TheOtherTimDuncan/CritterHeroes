using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
{
    public class DashboardModel
    {
        public string Target
        {
            get
            {
                return "SQL Server";
            }
        }

        public string Source
        {
            get
            {
                return "Rescue Groups";
            }
        }

        public Dictionary<DataSources, string> ListSources = new Dictionary<DataSources, string>()
        {
            { DataSources.Breed, "Breeds" },
            { DataSources.CritterStatus, "Statuses" }
        };
    }
}