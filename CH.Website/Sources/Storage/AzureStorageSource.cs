using System;
using System.Collections.Generic;
using System.Linq;
using CH.Azure;
using CH.Domain.Contracts;

namespace CH.Website.Sources.Storage
{
    public class AzureStorageSource : IStorageSource
    {
        public IStorageContext _storageContext;

        public AzureStorageSource(string tableName)
        {
            _storageContext = new AzureStorage(tableName);
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