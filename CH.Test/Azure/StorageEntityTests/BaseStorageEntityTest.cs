using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure;
using CH.Domain.Proxies.Configuration;

namespace CH.Test.Azure.StorageEntityTests
{
    public class BaseStorageEntityTest : BaseTest
    {
        public AzureStorage GetEntityAzureStorage()
        {
            return GetAzureStorage("fflah");
        }

        public AzureStorage GetAzureStorage(string tableName)
        {
            return new AzureStorage(tableName, new AzureConfiguration());
        }
    }
}
