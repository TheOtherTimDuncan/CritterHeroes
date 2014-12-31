using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.StateManagement;

namespace CH.Domain.Services.Queries
{
    public class DashboardStatusQuery<T> where T : class, IDataItem<T>
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
