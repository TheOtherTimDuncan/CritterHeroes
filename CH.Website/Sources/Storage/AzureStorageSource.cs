using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Dashboard;

namespace CH.Website.Sources.Storage
{
    public class AzureStorageSource : IStorageSource
    {
        public AzureStorageSource()
        {
        }

        public int ID
        {
            get
            {
                return 0;
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