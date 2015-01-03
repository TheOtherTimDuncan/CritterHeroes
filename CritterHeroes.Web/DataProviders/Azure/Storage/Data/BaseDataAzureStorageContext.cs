using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Configuration;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Data
{
    public abstract class BaseDataAzureStorageContext<T> : BaseAzureStorageContext<T> where T : class
    {
        public BaseDataAzureStorageContext(IAzureConfiguration azureConfiguration)
            : base("listdata", azureConfiguration)
        {
        }

    }
}
