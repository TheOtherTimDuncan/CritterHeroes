using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.Common.Services.CommandHandlers.Dashboard
{
    public class DashboardStatusCommandHandler<T> : IDashboardStatusCommandHandler<T> where T : class, IDataItem<T>
    {
        private IMasterStorageContext<T> _target;
        private ISecondaryStorageContext<T> _source;

        public DashboardStatusCommandHandler(IMasterStorageContext<T> target, ISecondaryStorageContext<T> source)
        {
            this._source = source;
            this._target = target;
        }

        public async Task<CommandResult> ExecuteAsync(DashboardStatusCommand<T> command)
        {
            await _target.DeleteAllAsync();
            IEnumerable<T> data = await GetSourceItems(command, _source);
            await _target.SaveAsync(data);
            return CommandResult.Success();
        }

        protected virtual async Task<IEnumerable<T>> GetSourceItems(DashboardStatusCommand<T> command, IStorageContext<T> storageContext)
        {
            return await storageContext.GetAllAsync();
        }
    }
}
