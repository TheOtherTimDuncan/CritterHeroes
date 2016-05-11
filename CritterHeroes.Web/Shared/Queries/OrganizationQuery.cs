using System;
using System.Collections.Generic;
using CritterHeroes.Web.Shared.StateManagement;
using CritterHeroes.Web.Contracts.Queries;

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
