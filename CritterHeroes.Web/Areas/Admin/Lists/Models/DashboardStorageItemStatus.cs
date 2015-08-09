using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
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
