using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Lists.CommandHandlers
{
    public class BreedDashboardStatusCommandHandler : DashboardStatusCommandHandler<Breed>
    {
        public BreedDashboardStatusCommandHandler(IAzureStorageContext<Breed> target, IRescureGroupsStorageContext<Breed> source)
            :base(target,source)
        {
        }

        protected override async Task<IEnumerable<Breed>> GetSourceItems(DashboardStatusCommand<Breed> command, IStorageContext<Breed> storageContext)
        {
            return (await storageContext.GetAllAsync()).Where(x => command.OrganizationContext.SupportedCritters.Any(s => s.Name == x.Species));
        }
    }
}
