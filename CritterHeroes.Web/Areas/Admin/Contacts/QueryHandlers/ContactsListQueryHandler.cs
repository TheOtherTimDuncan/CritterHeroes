using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Contacts.Models;
using CritterHeroes.Web.Areas.Admin.Contacts.Queries;
using CritterHeroes.Web.Areas.Common.Models;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Contacts.QueryHandlers
{
    public class ContactsListQueryHandler : IAsyncQueryHandler<ContactsListQuery, ContactsListModel>
    {
        private ISqlStorageContext<Person> _storagePersons;

        public ContactsListQueryHandler(ISqlStorageContext<Person> storagePersons)
        {
            this._storagePersons = storagePersons;
        }

        public async Task<ContactsListModel> RetrieveAsync(ContactsListQuery query)
        {
            ContactsListModel model = new ContactsListModel();

            var filteredContacts = _storagePersons.Entities;

            filteredContacts = filteredContacts.OrderBy(x => x.LastName);

            model.Paging = new PagingModel(filteredContacts.Count(), query);

            model.Contacts = await
            (
                from x in filteredContacts
                select new ContactModel()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                }
            ).TakePage(query.Page, model.Paging.PageSize).ToListAsync();

            return model;
        }
    }
}
