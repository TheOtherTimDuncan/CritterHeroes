using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Shared.Models;

namespace CritterHeroes.Web.Features.Admin.Contacts.Models
{
    public class PersonEditModel
    {
        public int PersonID
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public bool IsEmailConfirmed
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

        public IEnumerable<StateOptionModel> StateOptions
        {
            get;
            set;
        }

        public string Zip
        {
            get;
            set;
        }
    }
}
