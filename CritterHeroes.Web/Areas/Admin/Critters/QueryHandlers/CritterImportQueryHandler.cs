using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.Areas.Admin.Critters.QueryHandlers
{
    public class CritterImportQueryHandler : IQueryHandler<CritterImportQuery, CritterImportModel>
    {
        private IRescueGroupsSearchStorage<CritterSearchResult> _critterStorage;

        public CritterImportQueryHandler(IRescueGroupsSearchStorage<CritterSearchResult> critterStorage)
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
