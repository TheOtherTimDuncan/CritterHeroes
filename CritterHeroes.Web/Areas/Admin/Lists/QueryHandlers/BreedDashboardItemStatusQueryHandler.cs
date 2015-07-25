using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Models.Status;

namespace CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers
{
    public class BreedDashboardItemStatusQueryHandler : BaseDashboardItemStatusQueryHandler<Breed>
    {
        public BreedDashboardItemStatusQueryHandler(ISqlStorageContext<Breed> source, IRescueGroupsStorageContext<Breed> target)
            : base(source, target)
        {
        }

        protected override void FillDataItem(DataItem dataItem, Breed source)
        {
            dataItem.Value = source.Species;
            if (source.BreedName != null)
            {
                dataItem.Value += " - " + source.BreedName;
            }
        }

        protected override IEnumerable<Breed> Sort(IEnumerable<Breed> source)
        {
            return source.OrderBy(x => x.Species).ThenBy(x => x.BreedName);
        }

        protected override async Task<DataResult> GetSourceItems(DashboardStatusQuery<Breed> query, IStorageContext<Breed> storageContext)
        {
            DataResult result = await base.GetSourceItems(query, storageContext);
            result.Items = result.Items.Where(x => query.OrganizationContext.SupportedCritters.Any(s => s.Name == x.Species));
            return result;
        }
    }
}
