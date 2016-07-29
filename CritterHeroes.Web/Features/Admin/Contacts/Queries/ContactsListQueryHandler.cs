using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using CritterHeroes.Web.Features.Shared.ActionExtensions;
using CritterHeroes.Web.Features.Shared.Models;
using TOTD.EntityFramework;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Features.Admin.Contacts.Queries
{
    public class ContactsListQuery : BaseContactsQuery, IAsyncQuery<ContactsListModel>
    {
    }

    public class ContactsListQueryHandler : IAsyncQueryHandler<ContactsListQuery, ContactsListModel>
    {
        private IContactsStorageContext _storageContacts;
        private IUrlGenerator _urlGenerator;

        public ContactsListQueryHandler(IContactsStorageContext storageContacts, IUrlGenerator urlGenerator)
        {
            this._storageContacts = storageContacts;
            this._urlGenerator = urlGenerator;
        }

        public async Task<ContactsListModel> ExecuteAsync(ContactsListQuery query)
        {
            ContactsListModel model = new ContactsListModel();

            var filteredPeople = _storageContacts.People;
            var filteredBusinesses = _storageContacts.Businesses;

            if (query.Status.IsNullOrEmpty() || query.Status.SafeEquals(ContactsQuery.StatusKeys.Active))
            {
                filteredPeople = filteredPeople.Where(x => x.IsActive == true);
            }
            else if (query.Status.SafeEquals(ContactsQuery.StatusKeys.Inactive))
            {
                filteredPeople = filteredPeople.Where(x => x.IsActive == false);
            }

            if (query.GroupID != null)
            {
                filteredPeople = filteredPeople.Where(x => x.Groups.Any(g => g.GroupID == query.GroupID));
                filteredBusinesses = filteredBusinesses.Where(x => x.Groups.Any(g => g.GroupID == query.GroupID));
            }

            var queryPeople = filteredPeople.Select(x => new
            {
                x.ID,
                ContactName = x.LastName + (x.FirstName != null && x.LastName != null ? ", " : "") + x.FirstName,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Zip = x.Zip,
                Email = x.Email,
                IsActive = x.IsActive,
                IsPerson = true,
                IsBusiness = false
            });

            var queryBusinesses = filteredBusinesses.Select(x => new
            {
                x.ID,
                ContactName = x.Name,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Zip = x.Zip,
                Email = x.Email,
                IsActive = true,
                IsPerson = false,
                IsBusiness = true
            });

            var contactsQuery = (
                query.Show.SafeEquals(ContactsQuery.ShowKeys.People) ? queryPeople :
                query.Show.SafeEquals(ContactsQuery.ShowKeys.Businesses) ? queryBusinesses :
                queryPeople.Concat(queryBusinesses));

            contactsQuery = contactsQuery.OrderBy(x => x.ContactName);

            model.Paging = new PagingModel(contactsQuery.Count(), query);

            model.Contacts = await
            (
                from x in contactsQuery
                select new ContactModel()
                {
                    ContactID = x.ID,
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
            ).TakePageToListAsync(query.Page, model.Paging.PageSize);

            IEnumerable<int> personIDs = model.Contacts.Where(x => x.IsPerson).Select(x => x.ContactID);
            var personGroups = await _storageContacts.People
                .Where(x => personIDs.Contains(x.ID))
                .SelectMany(x => x.Groups)
                .OrderBy(x => x.Group.Name)
                .SelectToListAsync(x => new
                {
                    x.PersonID,
                    x.Group.Name
                });

            IEnumerable<int> businessIDs = model.Contacts.Where(x => x.IsBusiness).Select(x => x.ContactID);
            var businessGroups = await _storageContacts.Businesses
                .Where(x => businessIDs.Contains(x.ID))
                .SelectMany(x => x.Groups)
                .OrderBy(x => x.Group.Name)
                .SelectToListAsync(x => new
                {
                    x.BusinessID,
                    x.Group.Name
                });

            foreach (ContactModel contactModel in model.Contacts)
            {
                if (contactModel.IsPerson)
                {
                    contactModel.Groups = string.Join(", ", personGroups.Where(x => x.PersonID == contactModel.ContactID).Select(x => x.Name));
                    contactModel.EditUrl = _urlGenerator.GeneratePersonEditUrl(contactModel.ContactID);
                }
                else
                {
                    contactModel.Groups = string.Join(", ", businessGroups.Where(x => x.BusinessID == contactModel.ContactID).Select(x => x.Name));
                    contactModel.EditUrl = _urlGenerator.GenerateBusinessEditUrl(contactModel.ContactID);
                }
            }

            return model;
        }
    }
}
