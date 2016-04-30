using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Features.Common.Queries
{
    public class PagingQuery
    {
        public PagingQuery()
        {
        }

        public PagingQuery(int page)
        {
            this.Page = page;
        }

        public int? Page
        {
            get;
            set;
        }
    }
}
