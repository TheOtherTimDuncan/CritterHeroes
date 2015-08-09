using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists.CommandHandlers;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Lists.Sources
{
    public class BreedDataModelSource : DataModelSource
    {
        public BreedDataModelSource(int sourceID, string title)
            : base(sourceID, title)
        {
        }

        public override async Task<DashboardItemStatus> GetDashboardItemStatusAsync(IDependencyResolver dependencyResolver, IStorageSource source, IStorageSource target)
        {
            DashboardStatusQuery<Breed> query = new DashboardStatusQuery<Breed>(target, source);
            return await dependencyResolver.GetService<IDashboardStatusQueryHandler<Breed>>().RetrieveAsync(query);
        }

        public override async Task<CommandResult> ExecuteSyncAsync(IDependencyResolver dependencyResolver, OrganizationContext organizationContext)
        {
            DashboardStatusCommand<Breed> command = new DashboardStatusCommand<Breed>(organizationContext);
            return await dependencyResolver.GetService<IDashboardStatusCommandHandler<Breed>>().ExecuteAsync(command);
        }
    }
}
