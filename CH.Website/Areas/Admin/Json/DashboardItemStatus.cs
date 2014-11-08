using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Models.Status;

namespace CH.Website.Areas.Admin.Json
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