using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
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
