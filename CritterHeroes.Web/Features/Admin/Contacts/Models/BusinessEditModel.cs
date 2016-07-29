using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Features.Shared.Models;

namespace CritterHeroes.Web.Features.Admin.Contacts.Models
{
    public class BusinessEditModel
    {
        public int BusinessID
        {
            get;
            set;
        }

        public string Name
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
