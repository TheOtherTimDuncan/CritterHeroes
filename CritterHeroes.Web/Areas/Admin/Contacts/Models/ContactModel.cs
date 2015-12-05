using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Areas.Admin.Contacts.Models
{
    public class ContactModel
    {
        public string ContactName
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }
        
        public string City
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }
    }
}
