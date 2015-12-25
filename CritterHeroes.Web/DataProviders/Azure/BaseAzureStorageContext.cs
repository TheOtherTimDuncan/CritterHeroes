using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.Azure
{
    public abstract class BaseAzureStorageContext<T> : IAzureStorageContext<T> where T : class
    {
        private IAzureService _azureService;

        private string _tableName;

        public BaseAzureStorageContext(string tableName, IAzureService azureService)
        {
            ThrowIf.Argument.IsNullOrEmpty(tableName, nameof(tableName));
            ThrowIf.Argument.IsNull(azureService, nameof(azureService));

            this._tableName = tableName;
            this._azureService = azureService;
        }

        public abstract T FromStorage(DynamicTableEntity tableEntity);

        public virtual DynamicTableEntity ToStorage(T entity)
        {
            DynamicTableEntity tableEntity = new DynamicTableEntity(GetPartitionKey(), GetRowKey(entity));
            return tableEntity;
        }

        public async Task<T> GetAsync(string entityKey)
        {
            DynamicTableEntity tableEntity = await GetAsync(GetPartitionKey(), entityKey);
            if (tableEntity == null)
            {
                return null;
            }

            return FromStorage(tableEntity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var query =
                from entity in await _azureService.CreateTableQuery<DynamicTableEntity>(_tableName)
                where entity.PartitionKey == GetPartitionKey()
                select entity;

            List<T> result = new List<T>();
            foreach (DynamicTableEntity tableEntity in query)
            {
                result.Add(FromStorage(tableEntity));
            }
            return result;
        }

        public async Task SaveAsync(T entity)
        {
            DynamicTableEntity tableEntity = ToStorage(entity);
            TableOperation operation = TableOperation.InsertOrReplace(tableEntity);
            await _azureService.ExecuteTableOperationAsync(_tableName, operation);
        }

        public async Task SaveAsync(IEnumerable<T> entities)
        {
            IEnumerable<DynamicTableEntity> tableEntities = entities.Select(x => ToStorage(x)).ToList();
            await _azureService.ExecuteTableBatchOperationAsync(_tableName, tableEntities, (entity) => TableOperation.InsertOrReplace(entity));
        }

        public async Task DeleteAsync(T entity)
        {
            DynamicTableEntity tableEntity = await GetAsync(GetPartitionKey(), GetRowKey(entity));
            if (tableEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(tableEntity);
                await _azureService.ExecuteTableOperationAsync(_tableName, deleteOperation);
            }
        }

        public async Task DeleteAllAsync()
        {
            var entities =
                from entity in await _azureService.CreateTableQuery<DynamicTableEntity>(_tableName)
                where entity.PartitionKey == GetPartitionKey()
                select entity;

            await _azureService.ExecuteTableBatchOperationAsync(_tableName, entities, (entity) => TableOperation.Delete(entity));
        }

        protected abstract string GetRowKey(T entity);

        protected virtual string GetPartitionKey()
        {
            return typeof(T).Name;
        }

        private async Task<DynamicTableEntity> GetAsync(string partitionKey, string rowKey)
        {
            TableOperation operation = TableOperation.Retrieve<DynamicTableEntity>(partitionKey, rowKey);
            TableResult tableResult = await _azureService.ExecuteTableOperationAsync(_tableName, operation);
            return tableResult.Result as DynamicTableEntity;
        }
    }
}
