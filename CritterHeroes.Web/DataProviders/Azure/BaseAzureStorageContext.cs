using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.Azure
{
    public abstract class BaseAzureStorageContext<T> : IAzureStorageContext<T> where T : class
    {
        private const int batchSize = 100;
        private CloudTable _cloudTable = null;
        private IAzureConfiguration _configuration;
        private string _tableName;

        public BaseAzureStorageContext(string tableName, IAzureConfiguration azureConfiguration)
        {
            ThrowIf.Argument.IsNullOrEmpty(tableName, nameof(tableName));
            ThrowIf.Argument.IsNull(azureConfiguration, nameof(azureConfiguration));

            this._tableName = tableName;
            this._configuration = azureConfiguration;
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
            CloudTable cloudTable = await GetCloudTable();

            var query =
                from entity in cloudTable.CreateQuery<DynamicTableEntity>()
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

            CloudTable cloudTable = await GetCloudTable();

            TableOperation operation = TableOperation.InsertOrReplace(tableEntity);
            await cloudTable.ExecuteAsync(operation);
        }

        public async Task SaveAsync(IEnumerable<T> entities)
        {
            List<DynamicTableEntity> tableEntities = new List<DynamicTableEntity>();
            foreach (T entity in entities)
            {
                DynamicTableEntity tableEntity = ToStorage(entity);
                tableEntities.Add(tableEntity);
            }

            await ExecuteBatchAsync(tableEntities, (entity) =>
            {
                return TableOperation.InsertOrReplace(entity);
            });
        }

        public async Task DeleteAsync(T entity)
        {
            DynamicTableEntity tableEntity = await GetAsync(GetPartitionKey(), GetRowKey(entity));

            if (tableEntity != null)
            {
                CloudTable cloudTable = await GetCloudTable();
                TableOperation deleteOperation = TableOperation.Delete(tableEntity);
                await cloudTable.ExecuteAsync(deleteOperation);
            }
        }

        public async Task DeleteAllAsync()
        {
            CloudTable cloudTable = await GetCloudTable();
            var entities =
                from entity in cloudTable.CreateQuery<DynamicTableEntity>()
                where entity.PartitionKey == GetPartitionKey()
                select entity;

            await ExecuteBatchAsync(entities, (entity) =>
            {
                return TableOperation.Delete(entity);
            });
        }

        protected async Task<CloudTable> GetCloudTable()
        {
            if (_cloudTable != null)
            {
                return _cloudTable;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            _cloudTable = client.GetTableReference(_tableName);
            await _cloudTable.CreateIfNotExistsAsync();
            return _cloudTable;
        }

        protected abstract string GetRowKey(T entity);

        protected virtual string GetPartitionKey()
        {
            return typeof(T).Name;
        }

        private async Task<DynamicTableEntity> GetAsync(string partitionKey, string rowKey)
        {
            CloudTable table = await GetCloudTable();
            TableOperation operation = TableOperation.Retrieve<DynamicTableEntity>(partitionKey, rowKey);
            TableResult tableResult = await table.ExecuteAsync(operation);

            if (tableResult.Result == null)
            {
                return null;
            }

            return (DynamicTableEntity)tableResult.Result;
        }

        public async Task ExecuteBatchAsync(IEnumerable<ITableEntity> tableEntities, Func<ITableEntity, TableOperation> operation)
        {
            CloudTable cloudTable = await GetCloudTable();

            TableBatchOperation batchOperation = new TableBatchOperation();
            int batchCount = 0;
            foreach (ITableEntity tableEntity in tableEntities)
            {
                batchOperation.Add(operation(tableEntity));
                batchCount++;

                if (batchCount >= batchSize)
                {
                    IEnumerable<TableResult> results = await cloudTable.ExecuteBatchAsync(batchOperation);
                    batchOperation.Clear();
                    batchCount = 0;
                }
            }

            if (batchCount > 0)
            {
                await cloudTable.ExecuteBatchAsync(batchOperation);
            }
        }
    }
}
