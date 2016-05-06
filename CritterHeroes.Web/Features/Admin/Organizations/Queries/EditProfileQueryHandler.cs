using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Organizations.Models;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Features.Admin.Organizations.Queries
{
    public class EditProfileQuery : IAsyncQuery<EditProfileModel>
    {
        public string JsTime
        {
            get;
            set;
        }
    }

    public class EditProfileQueryHandler : IAsyncQueryHandler<EditProfileQuery, EditProfileModel>
    {
        private IAppConfiguration _appConfiguration;
        private ISqlStorageContext<Organization> _storageContext;
        private IOrganizationLogoService _logoService;

        public EditProfileQueryHandler(IAppConfiguration appConfiguration, ISqlStorageContext<Organization> storageContext, IOrganizationLogoService logoService)
        {
            this._appConfiguration = appConfiguration;
            this._storageContext = storageContext;
            this._logoService = logoService;
        }

        public async Task<EditProfileModel> ExecuteAsync(EditProfileQuery query)
        {
            Organization org = await _storageContext.Entities.FindByIDAsync(_appConfiguration.OrganizationID);

            string timeZone = org.TimeZoneID;
            if (timeZone.IsNullOrEmpty() && !query.JsTime.IsNullOrEmpty())
            {
                // Only need the time zone in parentheses at the end
                // Also need to convert daylight savings to standards
                int start = query.JsTime.IndexOf('(') + 1;
                timeZone = query.JsTime
                    .Substring(start, query.JsTime.Length - start - 1)
                    .Replace("Daylight", "Standard");
            }

            return new EditProfileModel()
            {
                Name = org.FullName,
                ShortName = org.ShortName,
                Email = org.EmailAddress,
                LogoUrl = _logoService.GetLogoUrl(),
                TimeZoneID = timeZone,
                TimeZoneOptions = TimeZoneInfo.GetSystemTimeZones().Select(x => new TimeZoneOption()
                {
                    Value = x.Id,
                    Text = x.DisplayName,
                    IsSelected = (x.Id == timeZone)
                }).ToList()
            };
        }
    }
}
