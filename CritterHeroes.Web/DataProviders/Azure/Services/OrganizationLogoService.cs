using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.Azure.Services
{
    public class OrganizationLogoService : IOrganizationLogoService
    {
        private ISqlStorageContext<Organization> _storageContext;
        private IStateManager<OrganizationContext> _organizationStateManager;
        private IAzureService _azureService;

        private const bool isPrivate = false;

        public OrganizationLogoService(IStateManager<OrganizationContext> organizationStateManager, IAzureService azureService, ISqlStorageContext<Organization> storageContext)
        {
            this._storageContext = storageContext;
            this._organizationStateManager = organizationStateManager;
            this._azureService = azureService;
        }

        public string GetLogoUrl()
        {
            OrganizationContext orgContext = _organizationStateManager.GetContext();
            return _azureService.CreateBlobUrl(orgContext.LogoFilename);
        }

        public async Task SaveLogo(Stream source, string filename, string contentType)
        {
            OrganizationContext orgContext = _organizationStateManager.GetContext();

            // Let's delete the original logo first if there is one
            if (!orgContext.LogoFilename.IsNullOrEmpty() && !orgContext.LogoFilename.SafeEquals(filename))
            {
                await _azureService.DeleteBlobAsync(orgContext.LogoFilename, isPrivate);
            }

            // Update the organization with the filename
            Organization org = await _storageContext.Entities.FindByIDAsync(orgContext.OrganizationID);
            org.LogoFilename = filename;
            await _storageContext.SaveChangesAsync();

            // Upload the new logo
            await _azureService.UploadBlobAsync(org.LogoFilename, isPrivate, contentType, source);
        }
    }
}
