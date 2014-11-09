using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Models;

namespace CH.Azure
{
    public class OrganizationAzureStorage : AzureStorage<Organization>, IStorageContext<Organization>
    {
        public OrganizationAzureStorage(IAzureConfiguration azureConfiguration)
            : base("organization", azureConfiguration)
        {
        }
    }
}
