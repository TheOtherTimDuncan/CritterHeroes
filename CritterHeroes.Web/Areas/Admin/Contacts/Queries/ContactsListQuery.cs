using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Contacts.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Contacts.Queries
{
    public class ContactsListQuery : BaseContactsQuery, IAsyncQuery<ContactsListModel>
    {
    }
}
