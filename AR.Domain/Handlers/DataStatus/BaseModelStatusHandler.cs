using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Contracts;
using AR.Domain.Models.Status;

namespace AR.Domain.Handlers.DataStatus
{
    public abstract class BaseModelStatusHandler<T> : IDataStatusHandler where T : class
    {
        public async Task<DataStatusModel> GetModelStatusAsync(params IStorageContext[] storageContexts)
        {
            // Get data items from each storage context
            IEnumerable<DataResult> dataResults = await Task.WhenAll
            (
                from x in storageContexts
                select GetDataResult(x)
            );

            // Merge data items into single list with no duplicates
            IEnumerable<T> master = Enumerable.Empty<T>();
            foreach (DataResult dataResult in dataResults)
            {
                master = master.Union(dataResult.Items);
            }

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

            return model;
        }

        protected abstract void FillDataItem(DataItem dataItem, T source);

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

        private async Task<DataResult> GetDataResult(IStorageContext storageContext)
        {
            return new DataResult()
            {
                StorageID = storageContext.ID,
                Items = await storageContext.GetAllAsync<T>()
            };
        }

        private class DataResult
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
