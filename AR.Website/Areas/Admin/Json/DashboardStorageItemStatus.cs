using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AR.Website.Areas.Admin.Json
{
    public class DashboardStorageItemStatus
    {
        public int StorageID
        {
            get;
            set;
        }

        public int ValidCount
        {
            get;
            set;
        }

        public int InvalidCount
        {
            get;
            set;
        }
    }
}