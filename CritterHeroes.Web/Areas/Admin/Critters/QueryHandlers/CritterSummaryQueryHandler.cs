using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
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


            model.StatusSummary = await
            (
                from x in _critterStorage.Entities
                group x by x.Status.Name into g
                select new StatusModel()
                {
                    Status = g.Key,
                    Count = g.Count()
                }
            ).ToListAsync();

            return model;
        }
    }
}
