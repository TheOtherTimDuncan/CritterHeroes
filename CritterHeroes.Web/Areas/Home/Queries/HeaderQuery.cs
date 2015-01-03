using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Home.Queries
{
    public class HeaderQuery : IAsyncQuery<HeaderModel>
    {
        public HeaderQuery(OrganizationContext organizationContext)
        {
            this.OrganizationContext = organizationContext;
        }

        public OrganizationContext OrganizationContext
        {
            get;
            private set;
        }
    }
}