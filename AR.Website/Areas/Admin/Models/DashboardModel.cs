using System;
using System.Collections.Generic;

namespace AR.Website.Areas.Admin.Models
{
    public class DashboardModel
    {
        public IEnumerable<DashboardItemColumn> Columns
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