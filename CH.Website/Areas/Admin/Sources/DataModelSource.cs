using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.Models.Json;
using CH.Domain.Services.Commands;
using CH.Domain.StateManagement;

namespace CH.Website.Areas.Admin.Sources
{
    public abstract class DataModelSource
    {
        public static readonly DataModelSource AnimalStatus = new AnimalStatusDataModelSource(0, "Statuses");
        public static readonly DataModelSource AnimalBreed = new BreedDataModelSource(1, "Breeds");

        protected DataModelSource(int sourceID, string title)
        {
            this.ID = sourceID;
            this.Title = title;
        }

        public int ID
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public abstract Task<DashboardItemStatus> GetDashboardItemStatusAsync(IDependencyResolver dependencyResolver, IStorageSource source, IStorageSource target, OrganizationContext organizationContext);
        public abstract Task<CommandResult> ExecuteSyncAsync(IDependencyResolver dependencyResolver, OrganizationContext organizationContext);

        public static DataModelSource FromID(int sourceID)
        {
            DataModelSource source = GetAll().FirstOrDefault(x => x.ID == sourceID);
            if (source == null)
            {
                throw new ArgumentOutOfRangeException("sourceID", string.Format("Invalid DataModelSource ID {0}", sourceID));
            }
            return source;
        }
        
        public static IEnumerable<DataModelSource> GetAll()
        {
            yield return AnimalStatus;
            yield return AnimalBreed;
        }
    }
}