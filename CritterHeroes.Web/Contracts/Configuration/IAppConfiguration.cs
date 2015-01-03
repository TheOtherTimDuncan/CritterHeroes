using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Configuration
{
    public interface IAppConfiguration
    {
        Guid OrganizationID
        {
            get;
        }

        string BlobBaseUrl
        {
            get;
        }
    }
}
