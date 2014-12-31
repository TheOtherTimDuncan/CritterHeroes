using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Storage;
using CH.Domain.Models.Data;
using CH.Domain.Models.Status;

namespace CH.Domain.Services.QueryHandlers.Dashboard
{
    public class AnimalStatusDashboardItemStatusQueryHandler : BaseDashboardItemStatusQueryHandler<AnimalStatus>
    {
        public AnimalStatusDashboardItemStatusQueryHandler(IMasterStorageContext<AnimalStatus> source, ISecondaryStorageContext<AnimalStatus> target)
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
