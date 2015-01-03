using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using Elmah;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureErrorLog : ErrorLog
    {
        private const string _defaultTableName = "errorlog";
        private string _tableName;

        public AzureErrorLog(IDictionary config)
        {
            ThrowIf.Argument.IsNull(config, "config");

            this.ConnectionString = GetConnectionString(config);
        }

        public AzureErrorLog(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString
        {
            get;
            private set;
        }

        protected string TableName
        {
            get
            {
                return _tableName ?? _defaultTableName;
            }
            set
            {
                _tableName = value;
            }
        }

        public override ErrorLogEntry GetError(string id)
        {
            ThrowIf.Argument.IsNullOrEmpty(id, "id");

            CloudTable table = GetCloudTable();

            DynamicTableEntity entity = table.CreateQuery<DynamicTableEntity>()
                .Where(x => x.RowKey == id)
                .Select(x => x)
                .FirstOrDefault();

            return CreateErrorLogEntry(id, entity);
        }

        public override int GetErrors(int pageIndex, int pageSize, System.Collections.IList errorEntryList)
        {
            ThrowIf.Argument.IsLessThan(pageIndex, "pageIndex", 0);
            ThrowIf.Argument.IsLessThan(pageSize, "pageSize", 0);

            // Default is to get all of today's errors
            DateTime now = DateTime.UtcNow;
            string start = PartitionKeyHelper.GetLoggingKeyForDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc));
            string end = PartitionKeyHelper.GetLoggingKeyForDate(new DateTime(now.Year, now.Month, now.Day, 23, 59, 0, DateTimeKind.Utc));

            //TODO: Improve this query
            CloudTable table = GetCloudTable();
            var entities =
                (
                    from e in table.CreateQuery<DynamicTableEntity>()
                    where e.PartitionKey.CompareTo(start) >= 0 && e.PartitionKey.CompareTo(end) <= 0
                    select e
                ).ToList();
            //.Take((pageIndex + 1) * pageSize).ToList()
            //.Skip(pageIndex * pageSize);

            foreach (DynamicTableEntity entity in entities)
            {
                ErrorLogEntry entry = CreateErrorLogEntry(entity.RowKey, entity);
                errorEntryList.Add(entry);
            }

            return entities.Count();
        }

        public override string Log(Error error)
        {
            string partitionKey = PartitionKeyHelper.GetLoggingKey();
            string errorID = Guid.NewGuid().ToString();

            DynamicTableEntity entity = new DynamicTableEntity(partitionKey, errorID);
            entity["HostName"] = new EntityProperty(error.HostName);
            entity["Type"] = new EntityProperty(error.Type);
            entity["ErrorXml"] = new EntityProperty(ErrorXml.EncodeString(error));
            entity["Message"] = new EntityProperty(error.Message);
            entity["StatusCode"] = new EntityProperty(error.StatusCode);
            entity["User"] = new EntityProperty(error.User);
            entity["Source"] = new EntityProperty(error.Source);
            entity["TimeUtc"] = new EntityProperty(error.Time.ToUniversalTime());

            CloudTable table = GetCloudTable();
            table.CreateIfNotExists();
            TableOperation insertOperation = TableOperation.Insert(entity);
            TableResult tableResult = table.Execute(insertOperation);

            return errorID;
        }

        protected string GetConnectionString(IDictionary config)
        {
            string connectionStringName = config["connectionStringName"] as string;
            if (connectionStringName != null)
            {
                return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }

            string connectionString = config["connectionString"] as string;
            if (connectionString != null)
            {
                return connectionString;
            }

            string appKey = config["connectionStringAppKey"] as string;
            if (appKey != null)
            {
                return ConfigurationManager.AppSettings[appKey];
            }

            throw new Elmah.ApplicationException("Connection string not found for Azure Error Log");
        }

        protected CloudTable GetCloudTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(TableName);
            table.CreateIfNotExists();
            return table;
        }

        protected virtual ErrorLogEntry CreateErrorLogEntry(string id, DynamicTableEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ErrorLogEntry(this, id, ErrorXml.DecodeString(entity["ErrorXml"].StringValue));
        }
    }
}
