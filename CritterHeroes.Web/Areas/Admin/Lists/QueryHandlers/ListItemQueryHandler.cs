using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Areas.Admin.Lists.Queries;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Lists.QueryHandlers
{
    public class ListItemQueryHandler : IAsyncQueryHandler<ListItemQuery, DashboardItemStatus>
    {
        private IDataMapperFactory _factory;

        public ListItemQueryHandler(IDataMapperFactory factory)
        {
            this._factory = factory;
        }

        public async Task<DashboardItemStatus> RetrieveAsync(ListItemQuery query)
        {
            IDataMapper dataMapper = _factory.Create(query.DataSource);
            DashboardItemStatus result = await dataMapper.GetDashboardItemStatusAsync();
            return result;
        }
    }
}
