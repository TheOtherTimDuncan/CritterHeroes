using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Contacts.Models;
using CritterHeroes.Web.Areas.Admin.Contacts.Queries;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Areas.Admin.Contacts.QueryHandlers
{
    public class ContactsQueryHandler : IAsyncQueryHandler<ContactsQuery, ContactsModel>
    {
        public IHttpUser _httpUser;
        public ISqlStorageContext<Group> _groupStorage;

        public ContactsQueryHandler(IHttpUser httpUser, ISqlStorageContext<Group> groupStorage)
        {
            this._httpUser = httpUser;
            this._groupStorage = groupStorage;
        }

        public async Task<ContactsModel> RetrieveAsync(ContactsQuery query)
        {
            ContactsModel model = new ContactsModel();

            model.Query = query;
            model.ShowImports = _httpUser.IsInRole(IdentityRole.MasterAdmin.Name);

            model.GroupItems = await _groupStorage.Entities.OrderBy(x => x.Name).SelectToListAsync(x => new GroupSelectOptionModel()
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
