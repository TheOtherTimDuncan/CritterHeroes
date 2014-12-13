using System;
using System.Collections.Generic;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Models.Logging;

namespace CH.Azure
{
    public class EmailLogAzureStorage : AzureStorage<EmailLog>, IStorageContext<EmailLog>
    {
        public EmailLogAzureStorage(IAzureConfiguration azureConfiguration)
            : base("emaillog", azureConfiguration)
        {
        }
    }
}
