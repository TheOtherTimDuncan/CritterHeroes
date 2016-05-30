using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Critters.Models;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class CritterEditQuery : IAsyncQuery<CritterEditModel>
    {
        public int CritterID
        {
            get;
            set;
        }
    }

    public class CritterEditQueryHandler : IAsyncQueryHandler<CritterEditQuery, CritterEditModel>
    {
        private ISqlStorageContext<Critter> _storageCritters;

        public CritterEditQueryHandler(ISqlStorageContext<Critter> storageCritters)
        {
            this._storageCritters = storageCritters;
        }

        public async Task<CritterEditModel> ExecuteAsync(CritterEditQuery query)
        {
            Critter critter = await _storageCritters.Entities.FindByIDAsync(query.CritterID);

            CritterEditModel model = new CritterEditModel()
            {
                ID = critter.ID,
                Name = critter.Name
            };

            return model;
        }
    }
}
