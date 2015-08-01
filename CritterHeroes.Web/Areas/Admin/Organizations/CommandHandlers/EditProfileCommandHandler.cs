using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Organizations.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Organizations.CommandHandlers
{
    public class EditProfileCommandHandler : IAsyncCommandHandler<EditProfileModel>
    {
        private IAppConfiguration _appConfiguration;
        private IStorageContext<Organization> _storageContext;
        private IOrganizationLogoService _logoService;
        private IStateManager<OrganizationContext> _stateManager;

        public EditProfileCommandHandler(IAppConfiguration appConfiguration, IStorageContext<Organization> storageContext, IOrganizationLogoService logoService, IStateManager<OrganizationContext> stateManager)
        {
            this._appConfiguration = appConfiguration;
            this._storageContext = storageContext;
            this._logoService = logoService;
            this._stateManager = stateManager;
        }

        public async Task<CommandResult> ExecuteAsync(EditProfileModel command)
        {
            Organization org = await _storageContext.GetAsync(_appConfiguration.OrganizationID.ToString());
            org.FullName = command.Name;
            org.ShortName = command.ShortName;
            org.EmailAddress = command.Email;

            await _storageContext.SaveAsync(org);

            if (command.LogoFile != null)
            {
                await _logoService.SaveLogo(command.LogoFile.InputStream, command.LogoFile.FileName, command.LogoFile.ContentType);

                OrganizationContext orgContext = _stateManager.GetContext();
                orgContext.LogoFilename = command.LogoFile.FileName;
                _stateManager.SaveContext(orgContext);
            }

            return CommandResult.Success();
        }
    }
}