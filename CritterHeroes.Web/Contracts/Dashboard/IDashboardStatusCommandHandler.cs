using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Common.Services.Commands;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDashboardStatusCommandHandler<T> : IAsyncCommandHandler<DashboardStatusCommand<T>> where T : class, IDataItem<T>
    {
    }
}
