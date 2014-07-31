using System;
using System.Collections.Generic;
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Models
{
    public class DashboardStorageItem
    {
        public DashboardStorageItem(StorageSource storageSource)
        {
            this.StorageSourceID = storageSource.Value;
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