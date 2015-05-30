using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Areas.Admin.Lists.Sources
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