using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Admin.Lists.Models
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