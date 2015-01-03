using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureEmailLogger : BaseAzureLoggerStorageContext<EmailLog>, IEmailLogger
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

            List<EmailLog> result = new List<EmailLog>();
            foreach (DynamicTableEntity tableEntity in entities)
            {
                result.Add(FromStorage(tableEntity));
            }
            return result;
        }

        public async Task LogEmailAsync(EmailLog emailLog)
        {
            await SaveAsync(emailLog);
        }

        public override EmailLog FromStorage(DynamicTableEntity tableEntity)
        {
            Guid logID;
            if (!Guid.TryParse(tableEntity.RowKey, out logID))
            {
                throw new AzureException("EmailLog has invalid ID: " + tableEntity.RowKey);
            }

            EmailMessage message = JsonConvert.DeserializeObject<EmailMessage>(tableEntity["Message"].StringValue);

            EmailLog result = new EmailLog(logID, tableEntity["WhenSentUtc"].DateTime.Value, message)
            {
                ForUserID = tableEntity.SafeGetEntityPropertyStringValue("ForUserID")
            };

            return result;
        }

        public override DynamicTableEntity ToStorage(EmailLog entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity["ForUserID"] = new EntityProperty(entity.ForUserID);
            tableEntity["WhenSentUtc"] = new EntityProperty(entity.WhenSentUtc);
            tableEntity["Message"] = new EntityProperty(JsonConvert.SerializeObject(entity.Message));

            return tableEntity;
        }

        protected override string GetRowKey(EmailLog entity)
        {
            return entity.ID.ToString();
        }
    }
}
