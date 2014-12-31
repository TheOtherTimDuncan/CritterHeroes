using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Commands;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Dashboard
{
    public interface IDashboardStatusCommandHandler<T> : IAsyncCommandHandler<DashboardStatusCommand<T>> where T : class, IDataItem<T>
    {
    }
}
