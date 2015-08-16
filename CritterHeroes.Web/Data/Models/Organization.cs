using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Models
{
    public class Organization
    {
        public Organization()
        {
            ID = Guid.NewGuid();
            SupportedCritters = Enumerable.Empty<Species>();
        }

        public Organization(Guid organizationID)
        {
            this.ID = organizationID;
        }

        public Guid ID
        {
            get;
            private set;
        }

        public string FullName
        {
            get;
            set;
        }

        public string ShortName
        {
            get;
            set;
        }

        public string AzureName
        {
            get;
            set;
        }

        public string LogoFilename
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }

        public IEnumerable<Species> SupportedCritters
        {
            get;
            set;
        }
    }
}
