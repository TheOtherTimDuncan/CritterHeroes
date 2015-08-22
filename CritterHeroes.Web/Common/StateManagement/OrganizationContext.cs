using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.StateManagement
{
    public class OrganizationContext
    {
        public static OrganizationContext FromOrganization(Organization organization)
        {
            return new OrganizationContext()
            {
                OrganizationID = organization.ID,
                FullName = organization.FullName,
                ShortName = organization.ShortName,
                AzureName = organization.AzureName,
                LogoFilename = organization.LogoFilename,
                EmailAddress = organization.EmailAddress,
                SupportedCritters = organization.SupportedCritters.Select(x => SpeciesContext.FromSpecies(x.Species)).ToList()
            };
        }

        public Guid OrganizationID
        {
            get;
            set;
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

        public IEnumerable<SpeciesContext> SupportedCritters
        {
            get;
            set;
        }
    }
}
