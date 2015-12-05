using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Areas.Common.Models;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Critters.QueryHandlers
{
    public class CrittersListQueryHandler : IAsyncQueryHandler<CrittersListQuery, CrittersListModel>
    {
        private ISqlStorageContext<Critter> _critterStorage;

        public CrittersListQueryHandler(ISqlStorageContext<Critter> critterStorage)
        {
            this._critterStorage = critterStorage;
        }

        public async Task<CrittersListModel> RetrieveAsync(CrittersListQuery query)
        {
            CrittersListModel model = new CrittersListModel();

            var critters = _critterStorage.Entities;

            if (query.StatusID != null)
            {
                critters = critters.Where(x => x.StatusID == query.StatusID.Value);
            }

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
                    SiteID = x.RescueGroupsID.ToString(),
                    FosterName = x.Foster.FirstName + " " + x.Foster.LastName,
                    PictureFilename = x.Pictures.FirstOrDefault(p => p.Picture.DisplayOrder == 1).Picture.Filename
                }
            ).TakePage(query.Page, model.Paging.PageSize).ToListAsync();

            return model;
        }
    }
}
