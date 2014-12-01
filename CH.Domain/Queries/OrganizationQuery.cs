using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Queries;

namespace CH.Domain.Queries
{
    public class OrganizationQuery : IQuery
    {
        public Guid OrganizationID
        {
            get;
            set;
        }
    }
}
