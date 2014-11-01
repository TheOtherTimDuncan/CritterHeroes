using System;
using System.Collections.Generic;
using System.Linq;
using AR.Domain.Contracts;

namespace AR.Domain.StateManagement
{
    public class OrganizationStateManager : StateManager<OrganizationContext>
    {
        public OrganizationStateManager(IHttpContext httpContext)
            : base(httpContext, "Organization")
        {
        }
    }
}
