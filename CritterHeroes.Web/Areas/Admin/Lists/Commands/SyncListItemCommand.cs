using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;
using CritterHeroes.Web.Areas.Admin.Lists.Models;

namespace CritterHeroes.Web.Areas.Admin.Lists.Commands
{
    public class SyncListItemCommand
    {
        public DataSources DataSource
        {
            get;
            set;
        }

        public DashboardItemStatus ItemStatus
        {
            get;
            set;
        }
    }
}
