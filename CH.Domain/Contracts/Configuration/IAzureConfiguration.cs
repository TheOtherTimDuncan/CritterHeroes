using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Configuration
{
    public interface IAzureConfiguration
    {
        string ConnectionString
        {
            get;
        }
    }
}
