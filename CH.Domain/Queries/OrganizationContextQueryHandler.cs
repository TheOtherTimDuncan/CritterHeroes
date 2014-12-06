using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Queries;
using CH.Domain.Models;
using CH.Domain.StateManagement;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Queries
{
    public class OrganizationContextQueryHandler : IQueryHandler<OrganizationQuery, OrganizationContext>
    {
        private IStorageContext<Organization> _storageContext;
        private IStateManager<OrganizationContext> _stateManager;

        public OrganizationContextQueryHandler(IStateManager<OrganizationContext> stateManager, IStorageContext<Organization> storageContext)
        {
            this._stateManager = stateManager;
            this._storageContext = storageContext;
        }

        public async Task<OrganizationContext> Retrieve(OrganizationQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");

            OrganizationContext organizationContext = _stateManager.GetContext();

            if (organizationContext == null)
            {
                Organization organization = await _storageContext.GetAsync(query.OrganizationID.ToString());
                organizationContext = OrganizationContext.FromOrganization(organization);
                _stateManager.SaveContext(organizationContext);
            }

            return organizationContext;
        }
    }
}
