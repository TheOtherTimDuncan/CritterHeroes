using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Lists.Commands;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Areas.Admin.Lists.CommandHandlers
{
    public class SyncListItemCommandHandler:IAsyncCommandHandler<SyncListItemCommand>
    {
        private IDataMapperFactory _factory;

        public SyncListItemCommandHandler(IDataMapperFactory factory)
        {
            this._factory = factory;
        }

        public async Task<CommandResult> ExecuteAsync(SyncListItemCommand command)
        {
            IDataMapper dataMapper = _factory.Create(command.DataSource);
            await dataMapper.CopySourceToTarget();
            command.ItemStatus = await dataMapper.GetDashboardItemStatusAsync();
            return CommandResult.Success();
        }
    }
}
