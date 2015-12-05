using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Common.Queries;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Critters.Queries
{
    public class CrittersListQuery : BaseCrittersQuery, IAsyncQuery<CrittersListModel>
    {
       
    }
}
