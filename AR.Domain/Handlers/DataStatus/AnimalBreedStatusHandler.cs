using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models.Data;
using AR.Domain.Models.Status;

namespace AR.Domain.Handlers.DataStatus
{
    public class AnimalBreedStatusHandler : BaseModelStatusHandler<AnimalBreed>
    {
        protected override void FillDataItem(DataItem dataItem, AnimalBreed source)
        {
            dataItem.Value = source.Species;
            if (source.BreedName != null)
            {
                dataItem.Value += " - " + source.BreedName;
            }
        }

        protected override IEnumerable<AnimalBreed> Sort(IEnumerable<AnimalBreed> source)
        {
            return source.OrderBy(x => x.Species).ThenBy(x => x.BreedName);
        }
    }
}
