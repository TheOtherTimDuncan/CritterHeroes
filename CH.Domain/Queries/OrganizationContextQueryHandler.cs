using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Queries;
using CH.Domain.Models;
using CH.Domain.StateManagement;

namespace CH.Domain.Queries
{
    public class OrganizationContextQueryHandler : IQueryHandler<OrganizationQuery, OrganizationContext>
    {
        private IStateManager<OrganizationContext> _stateManager;
        private IStorageContext<Organization> _storageContext;

        public OrganizationContextQueryHandler(IStateManager<OrganizationContext> stateManager, IStorageContext<Organization> storageContext)
        {
            this._stateManager = stateManager;
            this._storageContext = storageContext;
        }

        public async Task<OrganizationContext> Retrieve(OrganizationQuery query)
        {
            OrganizationContext organizationContext = _stateManager.GetContext();
            if (organizationContext != null)
            {
                return organizationContext;
            }

            Organization organization = await _storageContext.GetAsync(query.OrganizationID.ToString());
            organizationContext = OrganizationContext.FromOrganization(organization);
            _stateManager.SaveContext(organizationContext);
            return organizationContext;
        }
    }
}
