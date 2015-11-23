using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureEmailLogger : BaseAzureLoggerStorageContext<EmailLog>, IEmailLogger
    {
        private IEmailStorageService _emailStorage;

        public AzureEmailLogger(IAzureConfiguration azureConfiguration, IEmailStorageService emailStorage)
            : base("emaillog", azureConfiguration)
        {
            this._emailStorage = emailStorage;
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

            List<EmailLog> result = new List<EmailLog>();
            foreach (DynamicTableEntity tableEntity in entities)
            {
                EmailLog emailLog = FromStorage(tableEntity);
                emailLog.EmailData = await _emailStorage.GetEmailAsync(emailLog.ID);
                result.Add(emailLog);
            }
            return result;
        }

        public async Task LogEmailAsync(EmailLog emailLog)
        {
            await SaveAsync(emailLog);
            await _emailStorage.SaveEmailAsync(emailLog.ID, emailLog.EmailData);
        }

        public override EmailLog FromStorage(DynamicTableEntity tableEntity)
        {
            Guid logID;
            if (!Guid.TryParse(tableEntity.RowKey, out logID))
            {
                throw new AzureException("EmailLog has invalid ID: " + tableEntity.RowKey);
            }

            EmailLog result = new EmailLog(logID, tableEntity[nameof(EmailLog.WhenCreatedUtc)].DateTimeOffsetValue.Value);

            return result;
        }

        public override DynamicTableEntity ToStorage(EmailLog entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity[nameof(EmailLog.WhenCreatedUtc)] = new EntityProperty(entity.WhenCreatedUtc);

            return tableEntity;
        }

        protected override string GetRowKey(EmailLog entity)
        {
            return entity.ID.ToString();
        }
    }
}
