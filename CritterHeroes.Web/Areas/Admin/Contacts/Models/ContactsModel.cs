using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Contacts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Contacts.Models
{
    public class ContactsModel
    {
        public bool ShowImports
        {
            get;
            set;
        }

        public ContactsQuery Query
        {
            get;
            set;
        }
    }
}
