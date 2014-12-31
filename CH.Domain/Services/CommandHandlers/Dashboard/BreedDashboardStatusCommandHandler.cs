using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Storage;
using CH.Domain.Models.Data;
using CH.Domain.Services.Commands;

namespace CH.Domain.Services.CommandHandlers.Dashboard
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
