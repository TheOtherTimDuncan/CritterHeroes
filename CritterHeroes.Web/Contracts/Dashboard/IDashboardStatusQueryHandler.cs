using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models.Json;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDashboardStatusQueryHandler<T> : IAsyncQueryHandler<DashboardStatusQuery<T>, DashboardItemStatus>
        where T : class, IDataItem<T>
    {
    }
}
