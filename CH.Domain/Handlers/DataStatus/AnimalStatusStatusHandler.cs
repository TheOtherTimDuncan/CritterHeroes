using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Status;
using CH.Domain.Models.Data;

namespace CH.Domain.Handlers.DataStatus
{
    public class AnimalStatusStatusHandler : BaseModelStatusHandler<AnimalStatus>
    {
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
