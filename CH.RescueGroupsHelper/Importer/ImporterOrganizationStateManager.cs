using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Shared.StateManagement;

namespace CH.RescueGroupsHelper.Importer
{
    public class ImporterOrganizationStateManager : IStateManager<OrganizationContext>
    {
        public void ClearContext()
        {
        }

        public OrganizationContext GetContext()
        {
            return new OrganizationContext()
            {
                AzureName = "fflah"
            };
        }

        public void SaveContext(OrganizationContext Context)
        {
        }
    }
}
