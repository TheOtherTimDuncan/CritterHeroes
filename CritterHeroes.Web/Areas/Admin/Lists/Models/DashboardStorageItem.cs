using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
{
    public class DashboardStorageItem
    {
        public DashboardStorageItem(IStorageSource storageSource)
        {
            this.StorageSourceID = storageSource.ID;
            this.Title = storageSource.Title;
        }

        public int StorageSourceID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }
    }
}