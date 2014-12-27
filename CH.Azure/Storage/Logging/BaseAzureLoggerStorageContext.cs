using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure.Utility;
using CH.Domain.Contracts.Configuration;

namespace CH.Azure.Storage.Logging
{
    public abstract class BaseAzureLoggerStorageContext<T> : BaseAzureStorageContext<T> where T : class
    {
        public BaseAzureLoggerStorageContext(string tableName, IAzureConfiguration azureConfiguration)
            : base(tableName, azureConfiguration)
        {
        }

        protected override string GetPartitionKey()
        {
            return PartitionKeyHelper.GetLoggingKey();
        }
    }
}
