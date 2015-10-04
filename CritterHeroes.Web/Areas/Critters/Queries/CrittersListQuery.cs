using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Common.Queries;
using CritterHeroes.Web.Areas.Critters.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Critters.Queries
{
    public class CrittersListQuery : PagingQuery, IAsyncQuery<CrittersListModel>
    {
    }
}
