using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class CritterSummaryQuery : IAsyncQuery<CritterSummaryModel>
    {
    }

    public class CritterSummaryQueryHandler : IAsyncQueryHandler<CritterSummaryQuery, CritterSummaryModel>
    {
        private ISqlQueryStorageContext<Critter> _critterStorage;

        public CritterSummaryQueryHandler(ISqlQueryStorageContext<Critter> critterStorage)
        {
            this._critterStorage = critterStorage;
        }

        public async Task<CritterSummaryModel> ExecuteAsync(CritterSummaryQuery query)
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
