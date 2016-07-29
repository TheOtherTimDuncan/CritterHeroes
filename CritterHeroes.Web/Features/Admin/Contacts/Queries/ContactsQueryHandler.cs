using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Contacts.Models;
using TOTD.EntityFramework;

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

    public class ContactsQueryHandler : IAsyncQueryHandler<ContactsQuery, ContactsModel>
    {
        public ISqlQueryStorageContext<Group> _groupStorage;

        public ContactsQueryHandler(ISqlQueryStorageContext<Group> groupStorage)
        {
            this._groupStorage = groupStorage;
        }

        public async Task<ContactsModel> ExecuteAsync(ContactsQuery query)
        {
            ContactsModel model = new ContactsModel();

            model.Query = query;

            model.GroupItems = await _groupStorage.Entities
                .OrderBy(x => x.Name)
                .SelectToListAsync(x => new GroupSelectOptionModel()
                {
                    Value = x.ID,
                    Text = x.Name,
                    IsBusiness = x.IsBusiness,
                    IsPerson = x.IsPerson,
                    IsSelected = (x.ID == query.GroupID)
                });

            return model;
        }
    }
}
