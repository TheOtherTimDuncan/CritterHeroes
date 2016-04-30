using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Common.Queries;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class CrittersListQuery : BaseCrittersQuery, IAsyncQuery<CrittersListModel>
    {
       
    }
}
