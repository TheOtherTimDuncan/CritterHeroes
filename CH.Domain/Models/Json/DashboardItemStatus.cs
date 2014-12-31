using System;
using System.Collections.Generic;
using CH.Domain.Models.Status;

namespace CH.Domain.Models.Json
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