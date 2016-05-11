using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Shared.StateManagement
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
                RescueGroupsID = organization.RescueGroupsID,
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

        public IEnumerable<SpeciesContext> SupportedCritters
        {
            get;
            set;
        }
    }
}
