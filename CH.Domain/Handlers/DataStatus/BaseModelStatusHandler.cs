using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Models.Status;

namespace CH.Domain.Handlers.DataStatus
{
    public abstract class BaseModelStatusHandler<T> : IDataStatusHandler where T : class
    {
        public async Task<DataStatusModel> GetModelStatusAsync(StatusContext statusContext, IStorageSource source, IStorageSource target)
        {
            IEnumerable<DataResult> dataResults = await Task.WhenAll(GetSourceItems(statusContext, source), GetTargetItems(statusContext, target));

            // Merge data items into single list with no duplicates
            IEnumerable<T> master = Enumerable.Empty<T>();
            foreach (DataResult dataResult in dataResults)
            {
                master = master.Union(dataResult.Items);
            }
            master = Sort(master);

            DataStatusModel model = new DataStatusModel();
            model.Items =
                from r in dataResults
                select new StorageItem()
                {
                    StorageID = r.StorageID,
                    Items =
                        from m in master
                        select CreateDataItem(r.Items, m)
                };
            model.DataItemCount = master.Count();

            return model;
        }

        public virtual async Task<DataStatusModel> SyncModelAsync(StatusContext statusContext, IStorageSource source, IStorageSource target)
        {
            await target.StorageContext.DeleteAllAsync<T>();
            IEnumerable<T> data = await source.StorageContext.GetAllAsync<T>();
            await target.StorageContext.SaveAsync<T>(data);
            return await GetModelStatusAsync(statusContext, source, target);
        }

        protected abstract void FillDataItem(DataItem dataItem, T source);
        protected abstract IEnumerable<T> Sort(IEnumerable<T> source);

        protected virtual async Task<DataResult> GetSourceItems(StatusContext statusContext, IStorageSource storageSource)
        {
            return new DataResult()
            {
                StorageID = storageSource.ID,
                Items = await storageSource.StorageContext.GetAllAsync<T>()
            };
        }

        protected virtual async Task<DataResult> GetTargetItems(StatusContext statusContext, IStorageSource storageSource)
        {
            return new DataResult()
            {
                StorageID = storageSource.ID,
                Items = await storageSource.StorageContext.GetAllAsync<T>()
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
