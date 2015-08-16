using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Common.Commands;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDataMapper
    {
        Task<DashboardItemStatus> GetDashboardItemStatusAsync();
        Task<CommandResult> CopySourceToTarget();
    }
}
