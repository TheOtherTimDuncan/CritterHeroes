using System;
using System.Collections.Generic;

namespace AR.Website.Areas.Admin.Models
{
    public class DashboardItemColumn
    {
        public DashboardItemColumn(int storageSourceID, string title)
        {
            this.StorageSourceID = storageSourceID;
            this.Title = title;
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