using System;
using System.Collections.Generic;
using CritterHeroes.Web.Features.Admin.Contacts.Queries;

namespace CritterHeroes.Web.Features.Admin.Contacts.Models
{
    public class ContactsModel
    {
        public ContactsQuery Query
        {
            get;
            set;
        }

        public IEnumerable<GroupSelectOptionModel> GroupItems
        {
            get;
            set;
        }
    }
}
