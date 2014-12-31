using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Dashboard;
using CH.Domain.Contracts.Storage;
using CH.Domain.Services.Commands;

namespace CH.Domain.Services.CommandHandlers.Dashboard
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
