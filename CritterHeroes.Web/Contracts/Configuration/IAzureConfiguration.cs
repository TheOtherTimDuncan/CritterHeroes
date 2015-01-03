using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Configuration
{
    public interface IAzureConfiguration
    {
        string ConnectionString
        {
            get;
        }
    }
}
