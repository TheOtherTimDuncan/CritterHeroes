using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Configuration;

namespace CH.Azure.Storage.Data
{
    public abstract class BaseDataAzureStorageContext<T> : BaseAzureStorageContext<T> where T : class
    {
        public BaseDataAzureStorageContext(IAzureConfiguration azureConfiguration)
            : base("listdata", azureConfiguration)
        {
        }

    }
}
