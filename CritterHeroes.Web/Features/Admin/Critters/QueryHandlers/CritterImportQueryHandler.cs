using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;

namespace CritterHeroes.Web.Features.Admin.Critters.QueryHandlers
{
    public class CritterImportQueryHandler : IQueryHandler<CritterImportQuery, CritterImportModel>
    {
        private IRescueGroupsStorageContext<CritterSource> _critterStorage;

        public CritterImportQueryHandler(IRescueGroupsStorageContext<CritterSource> critterStorage)
        {
            _critterStorage = critterStorage;
        }

        public CritterImportModel Execute(CritterImportQuery query)
        {
            CritterImportModel model = new CritterImportModel();
            model.FieldNames = _critterStorage.Fields.Select(x => x.Name).ToList();
            return model;
        }
    }
}
