using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Models
{
    public class Organization
    {
        public Organization()
            : this(Guid.NewGuid())
        {
        }

        public Organization(Guid organizationID)
        {
            this.ID = organizationID;
            SupportedCritters = new HashSet<OrganizationSupportedCritter>();
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

        public int? RescueGroupsID
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

        public virtual ICollection<OrganizationSupportedCritter> SupportedCritters
        {
            get;
            set;
        }

        public void AddSupportedCritter(Species species)
        {
            OrganizationSupportedCritter supportedCritter = new OrganizationSupportedCritter(this.ID, species);
            SupportedCritters.Add(supportedCritter);
        }
    }
}
