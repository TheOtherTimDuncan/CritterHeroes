using System;
using System.Collections.Generic;
using CritterHeroes.Web.Features.Shared.Queries;

namespace CritterHeroes.Web.Features.Admin.Critters.Queries
{
    public class BaseCrittersQuery : PagingQuery
    {
        public int? StatusID
        {
            get;
            set;
        }
    }
}
