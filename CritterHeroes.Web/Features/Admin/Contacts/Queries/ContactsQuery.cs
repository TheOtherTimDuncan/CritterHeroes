using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Contacts.Models;

namespace CritterHeroes.Web.Features.Admin.Contacts.Queries
{
    public class ContactsQuery : BaseContactsQuery, IAsyncQuery<ContactsModel>
    {
        public class StatusKeys
        {
            public const string Any = "Any";
            public const string Active = "Active";
            public const string Inactive = "Inactive";
        }

        public class ShowKeys
        {
            public const string All = "All";
            public const string Businesses = "Businesses";
            public const string People = "People";
        }
    }
}
