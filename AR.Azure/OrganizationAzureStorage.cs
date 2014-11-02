using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Contracts;
using AR.Domain.Models;

namespace AR.Azure
{
    public class OrganizationAzureStorage : AzureStorage<Organization>, IStorageContext<Organization>
    {
        public OrganizationAzureStorage()
            : base("organization")
        {
        }
    }
}
