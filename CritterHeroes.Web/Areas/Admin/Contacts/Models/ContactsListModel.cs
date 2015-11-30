using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Common.Models;

namespace CritterHeroes.Web.Areas.Admin.Contacts.Models
{
    public class ContactsListModel
    {
        public PagingModel Paging
        {
            get;
            set;
        }

        public IEnumerable<ContactModel> Contacts
        {
            get;
            set;
        }
    }
}
