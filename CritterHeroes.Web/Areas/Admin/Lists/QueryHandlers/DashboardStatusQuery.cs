using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers
{
    public class DashboardStatusQuery<T> : IAsyncQuery<DashboardItemStatus> where T : class, IDataItem<T>
    {
        public DashboardStatusQuery(IStorageSource target, IStorageSource source)
        {
            this.Source = source;
            this.Target = target;
        }

        public IStorageSource Source
        {
            get;
            private set;
        }

        public IStorageSource Target
        {
            get;
            private set;
        }
    }
}
