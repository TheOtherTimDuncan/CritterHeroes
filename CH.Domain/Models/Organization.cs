using System;
using System.Collections.Generic;
using System.Linq;

namespace CH.Domain.Models
{
    public class Organization
    {
        public Organization()
        {
            ID = Guid.NewGuid();
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

        public IEnumerable<string> SupportedCritters
        {
            get;
            set;
        }
    }
}
