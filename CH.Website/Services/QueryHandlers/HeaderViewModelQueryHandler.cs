using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Configuration;
using CH.Domain.StateManagement;
using CH.Website.Models;

namespace CH.Website.Services.QueryHandlers
{
    public class HeaderViewModelQueryHandler : BaseViewModelQueryHandler<OrganizationContext, HeaderModel>
    {
        private IAppConfiguration _appConfiguration;

        public HeaderViewModelQueryHandler(IAppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
        }

        public override Task<HeaderModel> Retrieve(OrganizationContext query)
        {
            HeaderModel model = new HeaderModel();
            model.LogoUrl = GetBlobUrl(_appConfiguration.BlobBaseUrl, query.AzureName, query.LogoFilename);
            return Task.FromResult(model);
        }
    }
}