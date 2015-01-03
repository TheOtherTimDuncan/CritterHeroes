using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Common.Services.Queries
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
