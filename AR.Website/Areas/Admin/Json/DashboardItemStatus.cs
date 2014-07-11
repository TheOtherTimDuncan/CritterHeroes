using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Website.Areas.Admin.Json
{
    public class DashboardItemStatus
    {
        public IEnumerable<DashboardStorageItemStatus> StorageItems
        {
            get;
            set;
        }
    }
}