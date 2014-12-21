using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Queries;
using CH.Domain.Models;
using CH.Domain.StateManagement;
using Microsoft.Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Services.Queries
{
    public class OrganizationContextQueryHandler : IAsyncQueryHandler<OrganizationQuery, OrganizationContext>
    {
        private const string _key = "CritterHeroes.Organization";

        private IStorageContext<Organization> _storageContext;
        private IStateManager<OrganizationContext> _stateManager;
        private IOwinContext _owinContext;

        public OrganizationContextQueryHandler(IStateManager<OrganizationContext> stateManager, IStorageContext<Organization> storageContext, IOwinContext owinContext)
        {
            this._stateManager = stateManager;
            this._storageContext = storageContext;
            this._owinContext = owinContext;
        }

        public async Task<OrganizationContext> RetrieveAsync(OrganizationQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");

            // First check to see if it's already been cached in the OwinContext
            OrganizationContext organizationContext = _owinContext.Get<OrganizationContext>(_key);
            if (organizationContext != null)
            {
                return organizationContext;
            }

            // Next check the request
            organizationContext = _stateManager.GetContext();
            if (organizationContext != null)
            {
                // Cache it in the OwinContext
                _owinContext.Set(_key, organizationContext);

                return organizationContext;
            }

            // It must not exist at all yet so let's create it
            // It needs cached in both the request and the OwinContext
            Organization organization = await _storageContext.GetAsync(query.OrganizationID.ToString());
            organizationContext = OrganizationContext.FromOrganization(organization);
            _stateManager.SaveContext(organizationContext);
            _owinContext.Set(_key, organizationContext);

            return organizationContext;
        }
    }
}
