using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure.Storage;
using CH.Azure.Utility;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Azure.Logging
{
    public class AzureEmailLogger : AzureStorage<EmailLog>, IEmailLogger
    {
        public AzureEmailLogger(IAzureConfiguration azureConfiguration)
            : base("emaillog", azureConfiguration)
        {
        }

        public async Task<IEnumerable<EmailLog>> GetEmailLogAsync(DateTime dateFrom, DateTime dateTo)
        {
            string start = PartitionKeyHelper.GetLoggingKeyForDate(dateFrom);
            string end = PartitionKeyHelper.GetLoggingKeyForDate(dateTo);

            CloudTable table = await GetCloudTable();
            var entities =
                (
                    from e in table.CreateQuery<DynamicTableEntity>()
                    where e.PartitionKey.CompareTo(start) >= 0 && e.PartitionKey.CompareTo(end) <= 0
                    select e
                ).ToList();

            StorageEntity<EmailLog> storageEntity = StorageEntityFactory.GetStorageEntity<EmailLog>();

            List<EmailLog> result = new List<EmailLog>();
            foreach (DynamicTableEntity tableEntity in entities)
            {
                storageEntity.TableEntity = tableEntity;
                result.Add(storageEntity.Entity);
            }
            return result;
        }

        public async Task LogEmailAsync(EmailLog emailLog)
        {
            await SaveAsync(emailLog);
        }
    }
}
