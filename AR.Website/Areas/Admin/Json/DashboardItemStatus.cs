using System;
using System.Collections.Generic;
using System.Linq;
using AR.Domain.Models.Status;

namespace AR.Website.Areas.Admin.Json
{
    public class DashboardItemStatus
    {
        public StorageItem TargetItem
        {
            get;
            set;
        }

        public StorageItem SourceItem
        {
            get;
            set;
        }

        public int DataItemCount
        {
            get;
            set;
        }
    }
}