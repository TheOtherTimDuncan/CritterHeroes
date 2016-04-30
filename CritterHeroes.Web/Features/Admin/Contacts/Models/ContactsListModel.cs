using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Common.Models;

namespace CritterHeroes.Web.Features.Admin.Contacts.Models
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
