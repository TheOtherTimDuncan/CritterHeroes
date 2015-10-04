using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Common.Queries
{
    public class PagingQuery
    {
        public PagingQuery()
            : this(1)
        {
        }

        public PagingQuery(int page)
        {
            this.Page = page;
        }

        public int Page
        {
            get;
            set;
        }
    }
}
