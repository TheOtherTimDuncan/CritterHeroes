using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Storage;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.Areas.Admin.Lists.CommandHandlers
{
    public class DashboardStatusCommandHandler<T> : IDashboardStatusCommandHandler<T> where T : class, IDataItem<T>
    {
        private ISqlStorageContext<T> _target;
        private IRescureGroupsStorageContext<T> _source;

        public DashboardStatusCommandHandler(ISqlStorageContext<T> target, IRescureGroupsStorageContext<T> source)
        {
            this._source = source;
            this._target = target;
        }

        public async Task<CommandResult> ExecuteAsync(DashboardStatusCommand<T> command)
        {
            IEnumerable<T> entities = await _target.GetAllAsync();
            entities.NullSafeForEach((entity) =>
            {
                _target.Delete(entity);
            });

            IEnumerable<T> data = await GetSourceItems(command, _source);
            data.NullSafeForEach((entity) =>
            {
                _target.Add(entity);
            });

            await _target.SaveChangesAsync();

            return CommandResult.Success();
        }

        protected virtual async Task<IEnumerable<T>> GetSourceItems(DashboardStatusCommand<T> command, IStorageContext<T> storageContext)
        {
            return await storageContext.GetAllAsync();
        }
    }
}
