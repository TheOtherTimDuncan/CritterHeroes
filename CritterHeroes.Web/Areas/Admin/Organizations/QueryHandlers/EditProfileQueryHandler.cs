using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Organizations.Models;
using CritterHeroes.Web.Areas.Admin.Organizations.Queries;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Organizations.QueryHandlers
{
    public class EditProfileQueryHandler : IAsyncQueryHandler<EditProfileQuery, EditProfileModel>
    {
        private IAppConfiguration _appConfiguration;
        private IStorageContext<Organization> _storageContext;
        private IOrganizationLogoService _logoService;

        public EditProfileQueryHandler(IAppConfiguration appConfiguration, IStorageContext<Organization> storageContext, IOrganizationLogoService logoService)
        {
            this._appConfiguration = appConfiguration;
            this._storageContext = storageContext;
            this._logoService = logoService;
        }

        public async Task<EditProfileModel> RetrieveAsync(EditProfileQuery query)
        {
            Organization org = await _storageContext.GetAsync(_appConfiguration.OrganizationID.ToString());
            return new EditProfileModel()
            {
                Name = org.FullName,
                ShortName = org.ShortName,
                Email = org.EmailAddress,
                LogoUrl = _logoService.GetLogoUrl()
            };
        }
    }
}