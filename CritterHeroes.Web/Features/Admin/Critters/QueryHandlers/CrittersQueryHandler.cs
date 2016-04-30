using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Features.Admin.Critters.QueryHandlers
{
    public class CrittersQueryHandler : IAsyncQueryHandler<CrittersQuery, CrittersModel>
    {
        private ISqlStorageContext<CritterStatus> _statusStorage;
        private IHttpUser _user;

        public CrittersQueryHandler(ISqlStorageContext<CritterStatus> statusStorage, IHttpUser user)
        {
            this._statusStorage = statusStorage;
            this._user = user;
        }

        public async Task<CrittersModel> ExecuteAsync(CrittersQuery query)
        {
            CrittersModel model = new CrittersModel();

            model.Query = query;
            model.ShowImport = (_user.IsInRole(UserRole.MasterAdmin));

            model.StatusItems = await _statusStorage.Entities
                .OrderBy(x => x.Name)
                .SelectToListAsync(x => new SelectListItem()
                {
                    Value = x.ID.ToString(),
                    Text = x.Name,
                    Selected = (x.ID == query.StatusID)
                });

            return model;
        }
    }
}
