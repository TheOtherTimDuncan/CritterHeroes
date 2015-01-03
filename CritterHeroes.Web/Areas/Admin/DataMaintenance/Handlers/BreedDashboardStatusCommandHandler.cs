using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.Data;

namespace CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers
{
    public class BreedDashboardStatusCommandHandler : DashboardStatusCommandHandler<Breed>
    {
        public BreedDashboardStatusCommandHandler(IMasterStorageContext<Breed> target, ISecondaryStorageContext<Breed> source)
            :base(target,source)
        {
        }

        protected override async Task<IEnumerable<Breed>> GetSourceItems(DashboardStatusCommand<Breed> command, IStorageContext<Breed> storageContext)
        {
            return (await storageContext.GetAllAsync()).Where(x => command.OrganizationContext.SupportedCritters.Any(s => s.Name == x.Species));
        }
    }
}
