using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Contacts.Models;
using CritterHeroes.Web.Areas.Admin.Contacts.Queries;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Contacts.QueryHandlers
{
    public class ContactsQueryHandler : IAsyncQueryHandler<ContactsQuery, ContactsModel>
    {
        public IHttpUser _httpUser;

        public ContactsQueryHandler(IHttpUser httpUser)
        {
            this._httpUser = httpUser;
        }

        public async Task<ContactsModel> RetrieveAsync(ContactsQuery query)
        {
            ContactsModel model = new ContactsModel();

            model.Query = query;
            model.ShowImports = _httpUser.IsInRole(IdentityRole.MasterAdmin.Name);

            return model;
        }
    }
}
