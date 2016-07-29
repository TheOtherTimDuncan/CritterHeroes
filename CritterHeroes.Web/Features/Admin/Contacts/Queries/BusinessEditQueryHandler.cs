using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Shared.Models;
using TOTD.EntityFramework;

namespace CritterHeroes.Web.Features.Admin.Contacts.Queries
{
    public class BusinessEditQuery : IAsyncQuery<BusinessEditModel>
    {
        public int BusinessID
        {
            get;
            set;
        }
    }

    public class BusinessEditQueryHandler : IAsyncQueryHandler<BusinessEditQuery, BusinessEditModel>, IAsyncQueryRebuilder<BusinessEditModel>
    {
        private ISqlQueryStorageContext<Business> _storageBusinesses;
        private ISqlQueryStorageContext<State> _storageStates;

        public BusinessEditQueryHandler(ISqlQueryStorageContext<Business> storageBusinesses, ISqlQueryStorageContext<State> storageStates)
        {
            this._storageBusinesses = storageBusinesses;
            this._storageStates = storageStates;
        }

        public async Task<BusinessEditModel> ExecuteAsync(BusinessEditQuery query)
        {
            Business business = await _storageBusinesses.Entities.FindByIDAsync(query.BusinessID);
            if (business == null)
            {
                throw new InvalidOperationException($"Business ID {query.BusinessID} not found");
            }

            BusinessEditModel model = new BusinessEditModel()
            {
                BusinessID = business.ID,
                Name = business.Name,
                Address = business.Address,
                City = business.City,
                State = business.State,
                Zip = business.Zip
            };

            await RebuildAsync(model);

            return model;
        }

        public async Task RebuildAsync(BusinessEditModel model)
        {
            model.StateOptions = await _storageStates.Entities
                .OrderBy(x => x.Name)
                .SelectToListAsync(x => new StateOptionModel()
                {
                    Value = x.Abbreviation,
                    Text = x.Name,
                    IsSelected = (x.Abbreviation == model.State)
                });
        }
    }
}
