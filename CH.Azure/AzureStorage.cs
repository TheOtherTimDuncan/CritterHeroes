using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure.Storage;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Azure
{
    public class AzureStorage : IStorageContext
    {
        private const int batchSize = 100;
        private CloudTable _cloudTable = null;
        private IAzureConfiguration _configuration;
        private string _tableName;

        public AzureStorage(string tableName,  IAzureConfiguration azureConfiguration)
        {
            ThrowIf.Argument.IsNullOrEmpty(tableName, "tableName");
            ThrowIf.Argument.IsNull(azureConfiguration, "azureConfiguration");

            this._tableName = tableName;
            this._configuration=azureConfiguration;
        }

        public async Task<T> GetAsync<T>(string entityKey) where T : class
        {
            StorageEntity<T> storageEntity = StorageEntityFactory.GetStorageEntity<T>();
            storageEntity.TableEntity = await GetAsync(storageEntity.PartitionKey, entityKey);
            return storageEntity.Entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            StorageEntity<T> storageEntity = StorageEntityFactory.GetStorageEntity<T>();
            CloudTable cloudTable = await GetCloudTable();

            var query =
                from entity in cloudTable.CreateQuery<DynamicTableEntity>()
                where entity.PartitionKey == storageEntity.PartitionKey
                select entity;

            List<T> result = new List<T>();
            foreach (DynamicTableEntity tableEntity in query)
            {
                storageEntity.TableEntity = tableEntity;
                result.Add(storageEntity.Entity);
            }
            return result;
        }

        public async Task SaveAsync<T>(T entity) where T : class
        {
            StorageEntity<T> storageEntity = StorageEntityFactory.GetStorageEntity<T>();
            storageEntity.Entity = entity;

            CloudTable cloudTable = await GetCloudTable();

            TableOperation operation = TableOperation.InsertOrReplace(storageEntity.TableEntity);
            await cloudTable.ExecuteAsync(operation);
        }

        public async Task SaveAsync<T>(IEnumerable<T> entities) where T : class
        {
            StorageEntity<T> storageEntity = StorageEntityFactory.GetStorageEntity<T>();

            List<DynamicTableEntity> tableEntities = new List<DynamicTableEntity>();
            foreach (T entity in entities)
            {
                storageEntity.Entity = entity;
                tableEntities.Add(storageEntity.TableEntity);
            }

            await ExecuteBatchAsync(tableEntities, (entity) =>
            {
                return TableOperation.InsertOrReplace(entity);
            });
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            StorageEntity<T> storageEntity = StorageEntityFactory.GetStorageEntity<T>();
            storageEntity.Entity = entity;

            DynamicTableEntity tableEntity = await GetAsync(storageEntity.PartitionKey, storageEntity.RowKey);

            if (tableEntity != null)
            {
                CloudTable cloudTable = await GetCloudTable();
                TableOperation deleteOperation = TableOperation.Delete(tableEntity);
                await cloudTable.ExecuteAsync(deleteOperation);
            }
        }

        public async Task DeleteAllAsync<T>() where T : class
        {
            StorageEntity<T> storageEntity = StorageEntityFactory.GetStorageEntity<T>();

            CloudTable cloudTable = await GetCloudTable();
            var entities =
                from entity in cloudTable.CreateQuery<DynamicTableEntity>()
                where entity.PartitionKey == storageEntity.PartitionKey
                select entity;

            await ExecuteBatchAsync(entities, (entity) =>
            {
                return TableOperation.Delete(entity);
            });
        }

        private async Task<CloudTable> GetCloudTable()
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
