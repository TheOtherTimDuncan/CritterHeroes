using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class BusinessSource
    {
        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Company
        {
            get;
            set;
        }

        public string Email
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

        public string PhoneWork
        {
            get;
            set;
        }

        public string PhoneWorkExtension
        {
            get;
            set;
        }

        public string PhoneFax
        {
            get;
            set;
        }

        public IEnumerable<string> GroupNames
        {
            get;
            set;
        }
    }
}
