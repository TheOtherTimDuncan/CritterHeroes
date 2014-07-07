using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models.Status;
using AR.Domain.Models.Data;

namespace AR.Domain.Handlers.DataStatus
{
    public class AnimalStatusStatusHandler :BaseModelStatusHandler<AnimalStatus>
    {
        protected override void FillDataItem(DataItem dataItem, AnimalStatus source)
        {
            dataItem.Value = source.Name;
        }
    }
}
