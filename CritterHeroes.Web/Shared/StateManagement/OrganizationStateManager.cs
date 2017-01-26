using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using Microsoft.Owin;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.StateManagement
{
    public class OrganizationStateManager : StateManager<OrganizationContext>
    {
        public OrganizationStateManager(IOwinContext owinContext, IStateSerializer serializer)
            : base(owinContext, serializer, "Organization")
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

            if (context.EmailAddress.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
