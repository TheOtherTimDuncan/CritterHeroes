using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Critters.QueryHandlers
{
    public class CritterSummaryQueryHandler : IAsyncQueryHandler<CritterSummaryQuery, CritterSummaryModel>
    {
        private ISqlStorageContext<Critter> _critterStorage;

        public CritterSummaryQueryHandler(ISqlStorageContext<Critter> critterStorage)
        {
            this._critterStorage = critterStorage;
        }

        public async Task<CritterSummaryModel> RetrieveAsync(CritterSummaryQuery query)
        {
            CritterSummaryModel model = new CritterSummaryModel();

            model.StatusSummary = await _critterStorage.Entities
                .GroupBy(x => new
                {
                    x.StatusID,
                    x.Status.Name
                })
                .SelectToListAsync(x => new StatusModel()
                {
                    StatusID = x.Key.StatusID,
                    Status = x.Key.Name,
                    Count = x.Count()
                });

            return model;
        }
    }
}
