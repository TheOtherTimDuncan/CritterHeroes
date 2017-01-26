using System;
using System.Collections.Generic;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Shared.StateManagement;

namespace CritterHeroes.Web.Shared.Queries
{
    public class OrganizationQuery : IAsyncQuery<OrganizationContext>
    {
        public Guid OrganizationID
        {
            get;
            set;
        }
    }
}
