using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Azure.Mapping;
using AR.Domain.Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Azure
{
    public class AzureStorage : IStorageContext
    {
        private const int batchSize = 100;

        public AzureStorage(string tableName)
            : this(tableName, ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString)
        {
        }

        public AzureStorage(string tableName, string connectionString)
        {
            this.ConnectionString = connectionString;
            this.TableName = tableName;
        }

        public int ID
        {
            get;
            set;
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public async Task<T> GetAsync<T>(string entityID) where T : class
        {
            IAzureMapping<T> mapping = AzureMappingFactory.GetMapping<T>();

            CloudTable cloudTable = await GetCloudTable();

            DynamicTableEntity storageEntity = await GetAsync(cloudTable, mapping.PartitionKey, entityID);

            if (storageEntity == null)
            {
                return null;
            }

            T result = mapping.ToModel((DynamicTableEntity)storageEntity);
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            IAzureMapping<T> mapping = AzureMappingFactory.GetMapping<T>();
            CloudTable cloudTable = await GetCloudTable();

            var query =
                from entity in cloudTable.CreateQuery<DynamicTableEntity>()
                where entity.PartitionKey == mapping.PartitionKey
                select entity;

            return mapping.ToModel(query);
        }

        public async Task SaveAsync<T>(T entity) where T : class
        {
            IAzureMapping<T> mapping = AzureMappingFactory.GetMapping<T>();
            ITableEntity tableEntity = mapping.ToEntity(entity);

            CloudTable cloudTable = await GetCloudTable();

            TableOperation operation = TableOperation.InsertOrReplace(tableEntity);
            await cloudTable.ExecuteAsync(operation);
        }

        public async Task SaveAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            IAzureMapping<T> mapping = AzureMappingFactory.GetMapping<T>();
            ITableEntity tableEntity = mapping.ToEntity(entity);

            CloudTable cloudTable = await GetCloudTable();

            DynamicTableEntity storageEntity = await GetAsync(cloudTable, tableEntity.PartitionKey, tableEntity.RowKey);

            if (storageEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(storageEntity);
                await cloudTable.ExecuteAsync(deleteOperation);
            }
        }

        public async Task DeleteAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        private async Task<CloudTable> GetCloudTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(TableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        private async Task<DynamicTableEntity> GetAsync(CloudTable cloudTable, string partitionKey, string rowKey)
        {
            TableOperation operation = TableOperation.Retrieve<DynamicTableEntity>(partitionKey, rowKey);
            TableResult tableResult = await cloudTable.ExecuteAsync(operation);

            if (tableResult.Result == null)
            {
                return null;
            }

            return (DynamicTableEntity)tableResult.Result;
        }
    }
}
