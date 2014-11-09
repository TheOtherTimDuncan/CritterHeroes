using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Domain.StateManagement
{
    public class OrganizationStateManager : StateManager<OrganizationContext>
    {
        public OrganizationStateManager(IHttpContext httpContext)
            : base(httpContext, "Organization")
        {
        }

        public override OrganizationContext GetContext()
        {
            OrganizationContext result = base.GetContext();

            if (result != null)
            {
                // If any values are missing because of problems deserializing the cookie
                // return null so the cookie will be re-created

                if (result.AzureName.IsNullOrEmpty())
                {
                    return null;
                }

                if (result.FullName.IsNullOrEmpty())
                {
                    return null;
                }

                if (result.OrganizationID == null)
                {
                    return null;
                }

                if (result.ShortName == null)
                {
                    return null;
                }

                if (result.SupportedCritters.IsNullOrEmpty())
                {
                    return null;
                }
            }

            return result;
        }
    }
}
