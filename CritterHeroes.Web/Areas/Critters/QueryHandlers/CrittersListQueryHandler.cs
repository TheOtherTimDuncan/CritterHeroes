using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common.Models;
using CritterHeroes.Web.Areas.Critters.Models;
using CritterHeroes.Web.Areas.Critters.Queries;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Critters.QueryHandlers
{
    public class CrittersListQueryHandler : IAsyncQueryHandler<CrittersListQuery, CrittersListModel>
    {
        private ISqlStorageContext<Critter> _critterStorage;
        private ISqlStorageContext<CritterStatus> _statusStorage;

        public CrittersListQueryHandler(ISqlStorageContext<Critter> critterStorage, ISqlStorageContext<CritterStatus> statusStorage)
        {
            this._critterStorage = critterStorage;
            this._statusStorage = statusStorage;
        }

        public async Task<CrittersListModel> RetrieveAsync(CrittersListQuery query)
        {
            CrittersListModel model = new CrittersListModel()
            {
                Query = query
            };

            var critters = _critterStorage.Entities;

            model.Paging = new PagingModel(critters.Count(), query);

            critters = critters.OrderBy(x => x.Name);

            model.Critters = await
            (
                from x in critters
                select new CritterModel()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Status = x.Status.Name,
                    Breed = x.Breed.BreedName,
                    Sex = x.Sex,
                    FosterName = x.Person.FirstName + " " + x.Person.LastName,
                    PictureFilename = x.Pictures.FirstOrDefault(p => p.Picture.DisplayOrder == 1).Picture.Filename
                }
            ).TakePage(query.Page, model.Paging.PageSize).ToListAsync();

            model.StatusItems = await _statusStorage.Entities
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem()
                {
                    Value = x.ID.ToString(),
                    Text = x.Name
                })
                .ToListAsync();

            return model;
        }
    }
}
