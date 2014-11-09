using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CH.Domain.Contracts.Configuration;

namespace CH.Domain.Proxies.Configuration
{
    public class AzureConfiguration : IAzureConfiguration
    {
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            }
        }
    }
}
