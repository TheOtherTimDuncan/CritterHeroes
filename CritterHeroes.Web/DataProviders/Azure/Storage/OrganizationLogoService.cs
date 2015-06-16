using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class OrganizationLogoService : IOrganizationLogoService
    {
        private OrganizationContext _orgContext;
        private IAppConfiguration _appConfiguration;

        public OrganizationLogoService(OrganizationContext orgContext, IAppConfiguration appConfiguration)
        {
            this._orgContext = orgContext;
            this._appConfiguration = appConfiguration;
        }

        public string GetLogoUrl()
        {
            return GetBlobUrl(_orgContext.LogoFilename);
        }

        public void SaveLogo(Stream source, string filename)
        {
            throw new NotImplementedException();
        }

        public string GetTempLogoUrl()
        {
            return GetBlobUrl("temp/" + _orgContext.LogoFilename);
        }

        public void SaveTempLogo(Stream source, string filename)
        {
            throw new NotImplementedException();
        }

        private string GetBlobUrl(string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", _appConfiguration.BlobBaseUrl, _orgContext.AzureName.ToLower(), filename.ToLower());
        }
    }
}