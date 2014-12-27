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
using Newtonsoft.Json;

namespace CH.Azure.Storage.Logging
{
    public class AzureUserLogger : BaseAzureLoggerStorageContext<UserLog>, IUserLogger
    {
        public AzureUserLogger(IAzureConfiguration azureConfiguration)
            : base("userlog", azureConfiguration)
        {
        }

        public async Task<IEnumerable<UserLog>> GetUserLogAsync(DateTime dateFrom, DateTime dateTo)
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

            List<UserLog> result = new List<UserLog>();
            foreach (DynamicTableEntity tableEntity in entities)
            {
                result.Add(FromStorage(tableEntity));
            }
            return result;
        }

        public async Task LogActionAsync(UserActions userAction, string userName)
        {
            UserLog userLog = new UserLog(userAction, userName, DateTime.UtcNow);
            await SaveAsync(userLog);
        }

        public async Task LogActionAsync<T>(UserActions userAction, string userName, T additionalData)
        {
            UserLog userLog = new UserLog(userAction, userName, DateTime.UtcNow);
            userLog.AdditionalData = JsonConvert.SerializeObject(additionalData);
            await SaveAsync(userLog);
        }

        public override UserLog FromStorage(DynamicTableEntity tableEntity)
        {
            Guid logID;
            if (!Guid.TryParse(tableEntity.RowKey, out logID))
            {
                throw new AzureException("UserLog has invalid ID: " + tableEntity.RowKey);
            }

            UserActions userAction;
            string actionValue = tableEntity.SafeGetEntityPropertyStringValue("Action");
            if (!Enum.TryParse(actionValue, out userAction))
            {
                throw new AzureException("Invalid UserAction " + actionValue + " for UserLog ID " + tableEntity.RowKey);
            }

            DateTime? whenOccurred = tableEntity.SafeGetEntityPropertyDateTimeValue("WhenOccurredUtc");
            if (whenOccurred == null)
            {
                throw new AzureException("Invalid WhenOccurredUtc for UserLog ID " + tableEntity.RowKey);
            }

            UserLog result = new UserLog(logID, userAction, tableEntity.SafeGetEntityPropertyStringValue("Username"), whenOccurred.Value);
            result.AdditionalData = tableEntity.SafeGetEntityPropertyStringValue("AdditionalData");
            return result;
        }

        public override DynamicTableEntity ToStorage(UserLog entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity["Action"] = new EntityProperty(entity.Action.ToString());
            tableEntity["Username"] = new EntityProperty(entity.Username);
            tableEntity["WhenOccurredUtc"] = new EntityProperty(entity.WhenOccurredUtc);
            tableEntity["AdditionalData"] = new EntityProperty(entity.AdditionalData);

            return tableEntity;
        }

        protected override string GetRowKey(UserLog entity)
        {
            return entity.ID.ToString();
        }
    }
}
