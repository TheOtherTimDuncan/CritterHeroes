using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts
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
