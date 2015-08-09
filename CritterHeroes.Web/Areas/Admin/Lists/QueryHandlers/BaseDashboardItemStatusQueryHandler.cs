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

namespace CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers
{
    public abstract class BaseDashboardItemStatusQueryHandler<T> : IDashboardStatusQueryHandler<T> where T : class, IDataItem<T>
    {
        private ISqlStorageContext<T> _target;
        private IRescueGroupsStorageContext<T> _source;
        private IStateManager<OrganizationContext> _orgStateManager;

        public BaseDashboardItemStatusQueryHandler(ISqlStorageContext<T> target, IRescueGroupsStorageContext<T> source, IStateManager<OrganizationContext> orgStorageContext)
        {
            this._source = source;
            this._target = target;
            this._orgStateManager = orgStorageContext;
        }

        protected OrganizationContext OrganizationContext
        {
            get;
            private set;
        }

        public async Task<DashboardItemStatus> RetrieveAsync(DashboardStatusQuery<T> query)
        {
            this.OrganizationContext = _orgStateManager.GetContext();

            IEnumerable<DataResult> dataResults = await Task.WhenAll(GetSourceItems(query, _source), GetTargetItems(query, _target));

            // Merge data items into single list with no duplicates
            IEnumerable<T> master = Enumerable.Empty<T>();
            foreach (DataResult dataResult in dataResults)
            {
                master = master.Union(dataResult.Items);
            }
            master = Sort(master);

            IEnumerable<StorageItem> storageItems =
            (
                from r in dataResults
                select new StorageItem()
                {
                    StorageID = r.StorageID,
                    Items =
                        from m in master
                        select CreateDataItem(r.Items, m)
                }
            ).ToList();

            DashboardItemStatus model = new DashboardItemStatus();
            model.TargetItem = storageItems.First(x => x.StorageID == query.Target.ID);
            model.SourceItem = storageItems.First(x => x.StorageID == query.Source.ID);
            model.DataItemCount = master.Count();

            return model;
        }

        protected abstract void FillDataItem(DataItem dataItem, T source);
        protected abstract IEnumerable<T> Sort(IEnumerable<T> source);

        protected virtual async Task<DataResult> GetSourceItems(DashboardStatusQuery<T> query, IStorageContext<T> storageContext)
        {
            return new DataResult()
            {
                StorageID = query.Source.ID,
                Items = await storageContext.GetAllAsync()
            };
        }

        protected virtual async Task<DataResult> GetTargetItems(DashboardStatusQuery<T> query, ISqlStorageContext<T> storageContext)
        {
            return new DataResult()
            {
                StorageID = query.Target.ID,
                Items = await storageContext.GetAllAsync()
            };
        }

        private DataItem CreateDataItem(IEnumerable<T> sourceItems, T masterItem)
        {
            DataItem result = new DataItem();

            if (sourceItems.Contains(masterItem))
            {
                result.IsValid = true;
                FillDataItem(result, masterItem);
            }

            return result;
        }

        protected class DataResult
        {
            public int StorageID
            {
                get;
                set;
            }

            public IEnumerable<T> Items
            {
                get;
                set;
            }
        }
    }
}
