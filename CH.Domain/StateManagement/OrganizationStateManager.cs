using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Domain.StateManagement
{
    public class OrganizationStateManager : StateManager<OrganizationContext>
    {
        public OrganizationStateManager(IOwinContext owinContext)
            : base(owinContext, "Organization")
        {
        }

        protected override bool IsValid(OrganizationContext context)
        {
            if (context.AzureName.IsNullOrEmpty())
            {
                return false;
            }

            if (context.FullName.IsNullOrEmpty())
            {
                return false;
            }

            if (context.OrganizationID == null)
            {
                return false;
            }

            if (context.ShortName == null)
            {
                return false;
            }

            if (context.SupportedCritters.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
