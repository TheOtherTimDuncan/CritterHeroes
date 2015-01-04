using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common.CommandHandlers;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDashboardStatusCommandHandler<T> : IAsyncCommandHandler<DashboardStatusCommand<T>> where T : class, IDataItem<T>
    {
    }
}
