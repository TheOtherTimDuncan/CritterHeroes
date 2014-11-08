using System;
using System.Collections.Generic;

namespace CH.Website.Areas.Admin.Models
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

        public IEnumerable<DashboardItemModel> Items
        {
            get;
            set;
        }
    }
}