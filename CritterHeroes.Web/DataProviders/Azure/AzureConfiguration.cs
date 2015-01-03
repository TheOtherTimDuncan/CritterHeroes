using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CritterHeroes.Web.Contracts.Configuration;

namespace CritterHeroes.Web.DataProviders.Azure
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
