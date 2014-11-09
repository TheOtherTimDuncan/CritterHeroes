using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Configuration
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
