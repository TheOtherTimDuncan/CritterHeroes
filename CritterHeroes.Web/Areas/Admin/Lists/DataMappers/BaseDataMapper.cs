using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.Areas.Admin.Lists.DataMappers
{
    public abstract class BaseDataMapper<SourceType, TargetType> : IDataMapper
        where SourceType : class
        where TargetType : class
    {
        private IStateManager<OrganizationContext> _orgStateManager;

        public BaseDataMapper(ISqlStorageContext<TargetType> sqlStorageContext, IRescueGroupsStorageContext<SourceType> storageContext, IStateManager<OrganizationContext> orgStorageContext)
        {
            this.TargetStorageContext = sqlStorageContext;
            this.SourceStorageContext = storageContext;
            this._orgStateManager = orgStorageContext;
        }

        protected IStorageContext<SourceType> SourceStorageContext
        {
            get;
            private set;
        }
        protected ISqlStorageContext<TargetType> TargetStorageContext
        {
            get;
            private set;
        }

        public async Task<DashboardItemStatus> GetDashboardItemStatusAsync()
        {
            this.OrganizationContext = _orgStateManager.GetContext();

            IEnumerable<IEnumerable<string>> allItems = await Task.WhenAll(GetSourceItems(SourceStorageContext), GetTargetItems(TargetStorageContext));
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

            result.DataItemCount = masterItems.Count();

            return result;
        }

        public virtual async Task<CommandResult> CopySourceToTarget()
        {
            IEnumerable<TargetType> targets = await TargetStorageContext.GetAllAsync();
            targets.NullSafeForEach(x =>
            {
                TargetStorageContext.Delete(x);
            });

            IEnumerable<SourceType> sources = await SourceStorageContext.GetAllAsync();
            sources.NullSafeForEach(x =>
            {
                TargetType target = CreateTargetFromSource(x);
                TargetStorageContext.Add(target);
            });

            await TargetStorageContext.SaveChangesAsync();

            return CommandResult.Success();
        }

        protected OrganizationContext OrganizationContext
        {
            get;
            private set;
        }

        protected abstract Task<IEnumerable<string>> GetSourceItems(IStorageContext<SourceType> storageContext);
        protected abstract Task<IEnumerable<string>> GetTargetItems(ISqlStorageContext<TargetType> sqlStorageContext);
        protected abstract TargetType CreateTargetFromSource(SourceType source);

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

        protected string EmptyToNull(string value)
        {
            if (value == "")
            {
                return null;
            }

            return value;
        }
    }
}
