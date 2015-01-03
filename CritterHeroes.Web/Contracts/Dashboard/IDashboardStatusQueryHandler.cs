using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models.Json;
using CritterHeroes.Web.Models.Status;
using CritterHeroes.Web.Areas.Admin.DataMaintenance.Handlers;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDashboardStatusQueryHandler<T> : IAsyncQueryHandler<DashboardStatusQuery<T>, DashboardItemStatus>
        where T : class, IDataItem<T>
    {
    }
}
