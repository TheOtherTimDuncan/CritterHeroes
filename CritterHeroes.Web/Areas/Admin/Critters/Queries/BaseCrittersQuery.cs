using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Common.Queries;

namespace CritterHeroes.Web.Areas.Admin.Critters.Queries
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
