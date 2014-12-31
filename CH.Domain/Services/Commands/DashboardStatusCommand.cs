using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.StateManagement;

namespace CH.Domain.Services.Commands
{
    public class DashboardStatusCommand<T> where T : class, IDataItem<T>
    {
        public DashboardStatusCommand(OrganizationContext organizationContext)
        {
            this.OrganizationContext = organizationContext;
        }

        public OrganizationContext OrganizationContext
        {
            get;
            private set;
        }
    }
}
