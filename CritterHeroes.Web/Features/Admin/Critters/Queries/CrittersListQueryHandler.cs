using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Shared.ActionExtensions;
using CritterHeroes.Web.Features.Shared.Models;
using TOTD.EntityFramework;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class CrittersListQuery : BaseCrittersQuery, IAsyncQuery<CrittersListModel>
    {
    }

    public class CrittersListQueryHandler : IAsyncQueryHandler<CrittersListQuery, CrittersListModel>
    {
        private ISqlQueryStorageContext<Critter> _critterStorage;
        private IUrlGenerator _urlGenerator;

        public CrittersListQueryHandler(ISqlQueryStorageContext<Critter> critterStorage, IUrlGenerator urlGenerator)
        {
            this._critterStorage = critterStorage;
            this._urlGenerator = urlGenerator;
        }

        public async Task<CrittersListModel> ExecuteAsync(CrittersListQuery query)
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
            ).TakePageToListAsync(query.Page, model.Paging.PageSize);

            model.Critters.NullSafeForEach(x =>
            {
                x.EditUrl = _urlGenerator.GenerateAdminCrittersEditAction(x.ID);
            });

            return model;
        }
    }
}
