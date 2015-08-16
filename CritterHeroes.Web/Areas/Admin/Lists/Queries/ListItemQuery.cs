using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Lists.Queries
{
    public class ListItemQuery : IAsyncQuery<DashboardItemStatus>
    {
        public DataSources DataSource
        {
            get;
            set;
        }
    }
}
