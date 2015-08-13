using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Lists.Models;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDataMapper
    {
        Task<DashboardItemStatus> GetDashboardItemStatusAsync();
    }
}
