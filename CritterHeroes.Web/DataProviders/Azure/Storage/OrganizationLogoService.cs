using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class OrganizationLogoService : IOrganizationLogoService
    {
        private IAppConfiguration _appConfiguration;
        private IStateManager<OrganizationContext> _orgStateManager;

        private OrganizationContext _orgContext;

        public OrganizationLogoService(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
            this._orgStateManager = orgStateManager;
        }

        private OrganizationContext OrganizationContext
        {
            get
            {
                if (_orgContext == null)
                {
                    _orgContext = _orgStateManager.GetContext();
                }
                return _orgContext;
            }
        }

        public string GetLogoUrl()
        {
            return GetBlobUrl(OrganizationContext.LogoFilename);
        }

        public void SaveLogo(Stream source, string filename)
        {
            throw new NotImplementedException();
        }

        public string GetTempLogoUrl()
        {
            return GetBlobUrl("temp/" + OrganizationContext.LogoFilename);
        }

        public void SaveTempLogo(Stream source, string filename)
        {
            throw new NotImplementedException();
        }

        private string GetBlobUrl(string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", _appConfiguration.BlobBaseUrl, OrganizationContext.AzureName.ToLower(), filename.ToLower());
        }
    }
}