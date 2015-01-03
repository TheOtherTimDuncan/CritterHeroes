using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Common.Services.Commands
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
