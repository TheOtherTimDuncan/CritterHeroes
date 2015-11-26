using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Contacts.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Contacts.Queries
{
    public class ContactsListQuery : BaseContactsQuery, IAsyncQuery<ContactsListModel>
    {
    }
}
