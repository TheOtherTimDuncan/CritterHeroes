using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Data.Models
{
    public class BaseContact
    {
        protected BaseContact()
        {
        }

        public int ID
        {
            get;
            private set;
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

        public int? RescueGroupsID
        {
            get;
            set;
        }
    }
}
