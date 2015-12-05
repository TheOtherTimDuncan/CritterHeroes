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
using TOTD.Utility.StringHelpers;

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

            if (query.Status.IsNullOrEmpty() || query.Status.SafeEquals(ContactsQuery.StatusKeys.Active))
            {
                filteredContacts = filteredContacts.Where(x => x.IsActive == true);
            }
            else if (query.Status.SafeEquals(ContactsQuery.StatusKeys.Inactive))
            {
                filteredContacts = filteredContacts.Where(x => x.IsActive == false);
            }

            filteredContacts = filteredContacts.OrderBy(x => x.LastName);

            model.Paging = new PagingModel(filteredContacts.Count(), query);

            model.Contacts = await
            (
                from x in filteredContacts
                select new ContactModel()
                {
                    ContactName = x.LastName + (x.FirstName != null && x.LastName != null ? ", " : "") + x.FirstName,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Zip = x.Zip,
                    Email = x.Email,
                    IsActive = x.IsActive
                }
            ).TakePage(query.Page, model.Paging.PageSize).ToListAsync();

            return model;
        }
    }
}
