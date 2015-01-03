using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Common.StateManagement;

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
