using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts;

namespace CH.Domain.StateManagement
{
    public class OrganizationStateManager : StateManager<OrganizationContext>
    {
        public OrganizationStateManager(IHttpContext httpContext)
            : base(httpContext, "Organization")
        {
        }
    }
}
