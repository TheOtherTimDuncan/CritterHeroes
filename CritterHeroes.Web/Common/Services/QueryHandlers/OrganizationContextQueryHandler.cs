using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Services.Queries;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models;
using Microsoft.Owin;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers
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
