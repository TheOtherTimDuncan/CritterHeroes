using System;
using System.Collections.Generic;
using CritterHeroes.Web.Features.Common.Queries;

namespace CritterHeroes.Web.Features.Admin.Contacts.Queries
{
    public class BaseContactsQuery : PagingQuery
    {
        public string Status
        {
            get;
            set;
        }

        public string Show
        {
            get;
            set;
        }

        public int? GroupID
        {
            get;
            set;
        }
    }
}
