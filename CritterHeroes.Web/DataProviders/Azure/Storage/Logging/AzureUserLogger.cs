using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Models.Logging;
using Microsoft.Owin;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureUserLogger : BaseAzureLoggerStorageContext<UserLog>, IUserLogger
    {
        private IOwinContext _owinContext;

        public AzureUserLogger(IAzureConfiguration azureConfiguration, IOwinContext owinContext)
            : base("userlog", azureConfiguration)
        {
            this._owinContext = owinContext;
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
            await LogActionAsync(userAction, userName, (object)null);
        }

        public async Task LogActionAsync<T>(UserActions userAction, string userName, T additionalData)
        {
            UserLog userLog = new UserLog(userAction, userName, DateTimeOffset.UtcNow)
            {
                ThreadID = Thread.CurrentThread.ManagedThreadId,
                IPAddress = _owinContext.Request.RemoteIpAddress
            };

            if (additionalData != null)
            {
                userLog.AdditionalData = JsonConvert.SerializeObject(additionalData);
            }

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
            string actionValue = tableEntity.SafeGetEntityPropertyStringValue(nameof(UserLog.Action));
            if (!Enum.TryParse(actionValue, out userAction))
            {
                throw new AzureException("Invalid UserAction " + actionValue + " for UserLog ID " + tableEntity.RowKey);
            }

            DateTimeOffset? whenOccurred = tableEntity.SafeGetEntityPropertyDateTimeOffsetValue(nameof(UserLog.WhenOccurredUtc));
            if (whenOccurred == null)
            {
                throw new AzureException("Invalid WhenOccurredUtc for UserLog ID " + tableEntity.RowKey);
            }

            UserLog result = new UserLog(logID, userAction, tableEntity.SafeGetEntityPropertyStringValue(nameof(UserLog.Username)), whenOccurred.Value);
            result.AdditionalData = tableEntity.SafeGetEntityPropertyStringValue(nameof(UserLog.AdditionalData));
            result.IPAddress = tableEntity.SafeGetEntityPropertyStringValue(nameof(UserLog.IPAddress));
            result.ThreadID = tableEntity.SafeGetEntityPropertyIntValue(nameof(UserLog.ThreadID));

            return result;
        }

        public override DynamicTableEntity ToStorage(UserLog entity)
        {
            DynamicTableEntity tableEntity = base.ToStorage(entity);

            tableEntity[nameof(UserLog.Action)] = new EntityProperty(entity.Action.ToString());
            tableEntity[nameof(UserLog.Username)] = new EntityProperty(entity.Username);
            tableEntity[nameof(UserLog.WhenOccurredUtc)] = new EntityProperty(entity.WhenOccurredUtc);
            tableEntity[nameof(UserLog.ThreadID)] = new EntityProperty(entity.ThreadID);
            tableEntity[nameof(UserLog.IPAddress)] = new EntityProperty(entity.IPAddress);
            tableEntity[nameof(UserLog.AdditionalData)] = new EntityProperty(entity.AdditionalData);

            return tableEntity;
        }

        protected override string GetRowKey(UserLog entity)
        {
            return entity.ID.ToString();
        }
    }
}
