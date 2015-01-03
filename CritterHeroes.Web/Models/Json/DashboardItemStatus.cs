using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Status;

namespace CritterHeroes.Web.Models.Json
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