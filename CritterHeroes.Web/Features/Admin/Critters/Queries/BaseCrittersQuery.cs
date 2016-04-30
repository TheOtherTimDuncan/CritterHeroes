using System;
using System.Collections.Generic;
using CritterHeroes.Web.Features.Common.Queries;

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
