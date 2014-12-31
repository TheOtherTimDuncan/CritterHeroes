using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.Models.Data;
using CH.Domain.Models.Json;
using CH.Domain.Services.Commands;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;

namespace CH.Website.Areas.Admin.Sources
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