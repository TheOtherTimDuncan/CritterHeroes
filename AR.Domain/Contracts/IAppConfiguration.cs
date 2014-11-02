using System;
using System.Collections.Generic;

namespace AR.Domain.Contracts
{
    public interface IAppConfiguration
    {
        Guid OrganizationID
        {
            get;
        }
    }
}
