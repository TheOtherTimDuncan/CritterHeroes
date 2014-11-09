using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure;
using CH.Domain.Contracts.Configuration;

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
            return new AzureStorage(tableName, Using<IAzureConfiguration>());
        }
    }
}
