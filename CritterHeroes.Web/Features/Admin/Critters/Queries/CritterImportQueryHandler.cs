using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Features.Admin.Critters.Models;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class CritterImportQuery : IQuery<CritterImportModel>
    {
    }

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
