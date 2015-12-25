using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public abstract class BaseAzureLoggerStorageContext<T> : BaseAzureStorageContext<T> where T : class
    {
        private IAzureService _azureService;

        public BaseAzureLoggerStorageContext(string tableName, IAzureService azureService)
            : base(tableName, azureService)
        {
            this._azureService = azureService;
        }

        protected override string GetPartitionKey()
        {
            return _azureService.GetLoggingKey();
        }
    }
}
