using System;
using System.Collections.Generic;
using CH.Domain.Contracts.Queries;
using CH.Domain.Models.Json;
using CH.Domain.Models.Status;
using CH.Domain.Services.Queries;

namespace CH.Domain.Contracts.Dashboard
{
    public interface IDashboardStatusQueryHandler<T> : IAsyncQueryHandler<DashboardStatusQuery<T>, DashboardItemStatus>
        where T : class, IDataItem<T>
    {
    }
}
