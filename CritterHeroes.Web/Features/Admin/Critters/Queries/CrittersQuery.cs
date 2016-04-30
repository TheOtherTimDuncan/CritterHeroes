using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Critters.Models;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class CrittersQuery : BaseCrittersQuery, IAsyncQuery<CrittersModel>
    {
    }
}
