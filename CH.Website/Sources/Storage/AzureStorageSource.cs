using System;
using System.Collections.Generic;
using System.Linq;
using CH.Azure;
using CH.Dependency;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;

namespace CH.Website.Sources.Storage
{
    public class AzureStorageSource : IStorageSource
    {
        public IStorageContext _storageContext;

        public AzureStorageSource(string tableName)
        {
            _storageContext = new AzureStorage(tableName, DependencyContainer.Using<IAzureConfiguration>());
        }

        public int ID
        {
            get
            {
                return 0;
            }
        }

        public IStorageContext StorageContext
        {
            get
            {
                return _storageContext;
            }
        }

        public string Title
        {
            get
            {
                return "Azure";
            }
        }
    }
}