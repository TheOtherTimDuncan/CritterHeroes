using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Contracts.Configuration;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
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
