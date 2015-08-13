using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.Status;

namespace CritterHeroes.Web.Areas.Admin.Lists.DataMappers
{
    public abstract class BaseDataMapper<SourceType, TargetType> : IDataMapper
        where SourceType : class
        where TargetType : class
    {
        private ISqlStorageContext<TargetType> _sqlStorageContext;
        private IStorageContext<SourceType> _storageContext;
        private IStateManager<OrganizationContext> _orgStateManager;

        public BaseDataMapper(ISqlStorageContext<TargetType> sqlStorageContext, IStorageContext<SourceType> storageContext, IStateManager<OrganizationContext> orgStorageContext)
        {
            this._sqlStorageContext = sqlStorageContext;
            this._storageContext = storageContext;
            this._orgStateManager = orgStorageContext;
        }

        public async Task<DashboardItemStatus> GetDashboardItemStatusAsync()
        {
            this.OrganizationContext = _orgStateManager.GetContext();

            IEnumerable<IEnumerable<string>> allItems = await Task.WhenAll(GetSourceItems(_storageContext), GetTargetItems(_sqlStorageContext));
            IEnumerable<string> sourceItems = allItems.First();
            IEnumerable<string> targetItems = allItems.Last();
            IEnumerable<string> masterItems = sourceItems.Union(targetItems).OrderBy(x => x);

            DashboardItemStatus result = new DashboardItemStatus();

            result.SourceItem = new StorageItem()
            {
                Items = masterItems.Select(x => CreateDataItem(sourceItems, x))
            };

            result.TargetItem = new StorageItem()
            {
                Items = masterItems.Select(x => CreateDataItem(targetItems, x))
            };

            return result;
        }

        protected OrganizationContext OrganizationContext
        {
            get;
            private set;
        }

        protected abstract Task<IEnumerable<string>> GetSourceItems(IStorageContext<SourceType> storageContext);
        protected abstract Task<IEnumerable<string>> GetTargetItems(ISqlStorageContext<TargetType> sqlStorageContext);

        private DataItem CreateDataItem(IEnumerable<string> sourceItems, string masterItem)
        {
            DataItem result = new DataItem();

            if (sourceItems.Contains(masterItem))
            {
                result.IsValid = true;
                result.Value = masterItem;
            }

            return result;
        }
    }
}
