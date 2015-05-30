using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models.Json;

namespace CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers
{
    public class DashboardStatusQuery<T> : IAsyncQuery<DashboardItemStatus> where T : class, IDataItem<T>
    {
        public DashboardStatusQuery(IStorageSource target, IStorageSource source, OrganizationContext organizationContext)
        {
            this.Source = source;
            this.Target = target;
            this.OrganizationContext = organizationContext;
        }

        public OrganizationContext OrganizationContext
        {
            get;
            set;
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
