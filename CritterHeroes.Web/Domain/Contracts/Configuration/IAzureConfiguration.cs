using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts.Configuration
{
    public interface IAzureConfiguration
    {
        string ConnectionString
        {
            get;
        }
    }
}
