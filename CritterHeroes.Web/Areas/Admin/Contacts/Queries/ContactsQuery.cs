using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Admin.Contacts.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Contacts.Queries
{
    public class ContactsQuery : BaseContactsQuery, IAsyncQuery<ContactsModel>
    {
    }
}
