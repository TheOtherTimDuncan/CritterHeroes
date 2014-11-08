using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using CH.Domain.Models.Status;

namespace CH.Domain.Handlers.DataStatus
{
    public class BreedStatusHandler : BaseModelStatusHandler<Breed>
    {
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

        protected override async Task<DataResult> GetSourceItems(StatusContext statusContext, Contracts.IStorageSource storageSource)
        {
            DataResult result = await base.GetSourceItems(statusContext, storageSource);
            result.Items = result.Items.Where(x => statusContext.SupportedCritters.Any(s => s.Name == x.Species));
            return result;
        }
    }
}
