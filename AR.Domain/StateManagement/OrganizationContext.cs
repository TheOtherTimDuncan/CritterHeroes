using System;
using System.Collections.Generic;
using AR.Domain.Models;

namespace AR.Domain.StateManagement
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
                AzureTableName = organization.AzureTableName,
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

        public string AzureTableName
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
