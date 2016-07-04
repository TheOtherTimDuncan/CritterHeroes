using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Shared.StateManagement;

namespace CH.RescueGroupsImporter
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
