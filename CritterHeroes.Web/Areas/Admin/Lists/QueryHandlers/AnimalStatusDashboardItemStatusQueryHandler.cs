using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Models.Status;

namespace CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers
{
    public class AnimalStatusDashboardItemStatusQueryHandler : BaseDashboardItemStatusQueryHandler<AnimalStatus>
    {
        public AnimalStatusDashboardItemStatusQueryHandler(ISqlStorageContext<AnimalStatus> source, IRescureGroupsStorageContext<AnimalStatus> target)
            : base(source, target)
        {
        }

        protected override void FillDataItem(DataItem dataItem, AnimalStatus source)
        {
            dataItem.Value = source.Name;
        }

        protected override IEnumerable<AnimalStatus> Sort(IEnumerable<AnimalStatus> source)
        {
            return source.OrderBy(x => x.Name);
        }
    }
}
