using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Contracts;
using AR.Domain.Models;
using AR.Domain.StateManagement;

namespace AR.Domain.Handlers
{
    public class CreateOrganizationContext
    {
        private IAppConfiguration _appConfiguration;
        private IStateManager<OrganizationContext> _stateManager;
        private IStorageContext<Organization> _storageContext;

        public CreateOrganizationContext(IAppConfiguration appConfiguration, IStorageContext<Organization> storageContext, IStateManager<OrganizationContext> stateManager)
        {
            this._appConfiguration = appConfiguration;
            this._stateManager = stateManager;
            this._storageContext = storageContext;
        }

        public async Task<OrganizationContext> ExecuteAsync()
        {
            Organization organization =await  _storageContext.GetAsync(_appConfiguration.OrganizationID.ToString());
            OrganizationContext result = OrganizationContext.FromOrganization(organization);
            _stateManager.SaveContext(result);
            return result;
        }
    }
}
