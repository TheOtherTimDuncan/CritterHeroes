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
        private IContactsStorageContext _storageContacts;

        public ContactsListQueryHandler(IContactsStorageContext storageContacts)
        {
            this._storageContacts = storageContacts;
        }

        public async Task<ContactsListModel> RetrieveAsync(ContactsListQuery query)
        {
            ContactsListModel model = new ContactsListModel();

            var contactsQuery = _storageContacts.People.Select(x => new
            {
                ContactName = x.LastName + (x.FirstName != null && x.LastName != null ? ", " : "") + x.FirstName,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Zip = x.Zip,
                Email = x.Email,
                IsActive = x.IsActive,
                IsPerson = true,
                IsBusiness = false
            })
            .Concat(_storageContacts.Businesses.Select(x => new
            {
                ContactName = x.Name,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Zip = x.Zip,
                Email = x.Email,
                IsActive = true,
                IsPerson = false,
                IsBusiness = true
            }));

            if (query.Status.IsNullOrEmpty() || query.Status.SafeEquals(ContactsQuery.StatusKeys.Active))
            {
                contactsQuery = contactsQuery.Where(x => x.IsActive == true);
            }
            else if (query.Status.SafeEquals(ContactsQuery.StatusKeys.Inactive))
            {
                contactsQuery = contactsQuery.Where(x => x.IsActive == false);
            }

            contactsQuery = contactsQuery.OrderBy(x => x.ContactName);

            model.Paging = new PagingModel(contactsQuery.Count(), query);

            model.Contacts = await
            (
                from x in contactsQuery
                select new ContactModel()
                {
                    ContactName = x.ContactName,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Zip = x.Zip,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    IsPerson = x.IsPerson,
                    IsBusiness = x.IsBusiness
                }
            ).TakePage(query.Page, model.Paging.PageSize).ToListAsync();

            return model;
        }
    }
}
