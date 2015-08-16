using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
{
    public class DashboardModel
    {
        public DashboardStorageItem TargetStorageItem
        {
            get;
            set;
        }

        public DashboardStorageItem SourceStorageItem
        {
            get;
            set;
        }

        public Dictionary<DataSources, string> ListSources = new Dictionary<DataSources, string>()
        {
            { DataSources.Breed, "Breeds" },
            { DataSources.CritterStatus, "Statuses" }
        };
    }
}