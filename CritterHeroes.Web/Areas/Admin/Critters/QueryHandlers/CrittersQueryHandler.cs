using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Critters.QueryHandlers
{
    public class CrittersQueryHandler : IAsyncQueryHandler<CrittersQuery, CrittersModel>
    {
        private ISqlStorageContext<CritterStatus> _statusStorage;

        public CrittersQueryHandler(ISqlStorageContext<CritterStatus> statusStorage)
        {
            this._statusStorage = statusStorage;
        }

        public async Task<CrittersModel> RetrieveAsync(CrittersQuery query)
        {
            CrittersModel model = new CrittersModel();

            model.Query = query;

            model.StatusItems = await _statusStorage.Entities
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem()
                {
                    Value = x.ID.ToString(),
                    Text = x.Name,
                    Selected = (x.ID == query.StatusID)
                })
                .ToListAsync();

            return model;
        }
    }
}
