using System;
using System.Collections.Generic;
using AR.Domain.Contracts;

namespace AR.Website.Areas.Admin.Models
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