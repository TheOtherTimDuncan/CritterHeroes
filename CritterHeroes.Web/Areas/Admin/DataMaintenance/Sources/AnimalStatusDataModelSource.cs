using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Models.Data;
using CritterHeroes.Web.Models.Json;

namespace CritterHeroes.Web.Areas.Admin.Sources
{
    public class AnimalStatusDataModelSource : DataModelSource
    {
        public AnimalStatusDataModelSource(int sourceID, string title)
            : base(sourceID, title)
        {
        }

        public override async Task<DashboardItemStatus> GetDashboardItemStatusAsync(IDependencyResolver dependencyResolver, IStorageSource source, IStorageSource target, OrganizationContext organizationContext)
        {
            DashboardStatusQuery<AnimalStatus> query = new DashboardStatusQuery<AnimalStatus>(target, source, organizationContext);
            return await dependencyResolver.GetService<IDashboardStatusQueryHandler<AnimalStatus>>().RetrieveAsync(query);
        }

        public override async Task<CommandResult> ExecuteSyncAsync(IDependencyResolver dependencyResolver, OrganizationContext organizationContext)
        {
            DashboardStatusCommand<AnimalStatus> command = new DashboardStatusCommand<AnimalStatus>(organizationContext);
            return await dependencyResolver.GetService<IDashboardStatusCommandHandler<AnimalStatus>>().ExecuteAsync(command);
        }
    }
}