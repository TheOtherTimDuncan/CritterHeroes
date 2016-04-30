using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Contacts.Models;

namespace CritterHeroes.Web.Features.Admin.Contacts.Queries
{
    public class ContactsListQuery : BaseContactsQuery, IAsyncQuery<ContactsListModel>
    {
    }
}
