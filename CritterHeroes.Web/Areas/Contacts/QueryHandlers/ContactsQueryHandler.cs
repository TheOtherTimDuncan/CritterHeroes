using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Contacts.Models;
using CritterHeroes.Web.Areas.Contacts.Queries;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Contacts.QueryHandlers
{
    public class ContactsQueryHandler : IAsyncQueryHandler<ContactsQuery, ContactsModel>
    {
        public async Task<ContactsModel> RetrieveAsync(ContactsQuery query)
        {
            ContactsModel model = new ContactsModel();
            return model;
        }
    }
}
