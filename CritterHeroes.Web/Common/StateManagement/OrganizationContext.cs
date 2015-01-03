using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Data;

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
                SupportedCritters = organization.SupportedCritters
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

        public IEnumerable<Species> SupportedCritters
        {
            get;
            set;
        }
    }
}
