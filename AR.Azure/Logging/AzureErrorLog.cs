using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elmah;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace AR.Azure.Logging
{
    public class AzureErrorLog : ErrorLog
    {
        private const string _defaultTableName = "errorlog";
        private string _tableName;

        public AzureErrorLog(IDictionary config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

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
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            if (id.Length == 0)
            {
                throw new ArgumentException(null, "id");
            }

            CloudTable table = GetCloudTable();

            DynamicTableEntity entity = table.CreateQuery<DynamicTableEntity>()
                .Where(x => x.RowKey == id)
                .Select(x => x)
                .FirstOrDefault();

            return CreateErrorLogEntry(id, entity);
        }

        public override int GetErrors(int pageIndex, int pageSize, System.Collections.IList errorEntryList)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, null);
            }

            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, null);
            }

            //TODO: Improve this query
            CloudTable table = GetCloudTable();
            var entities =
                (
                    from e in table.CreateQuery<DynamicTableEntity>()
                    select e
                )
                .Take((pageIndex + 1) * pageSize).ToList()
                .Skip(pageIndex * pageSize);

            foreach (DynamicTableEntity entity in entities)
            {
                ErrorLogEntry entry = CreateErrorLogEntry(entity.RowKey, entity);
                errorEntryList.Add(entry);
            }

            return entities.Count();
        }

        public override string Log(Error error)
        {
            string errorID = (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks).ToString("d19");
            string partitionKey = error.Time.ToString("yyyyMMddHH");

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
