using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure.Utility;
using CH.Domain.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Azure.Storage.Logging
{
    public class UserLogStorageEntity : StorageEntity<UserLog>
    {
        public override string PartitionKey
        {
            get
            {
                return PartitionKeyHelper.GetLoggingKey();
            }
        }
        public override string RowKey
        {
            get
            {
                return Entity.ID.ToString();
            }
        }

        protected override void CopyEntityToStorage(DynamicTableEntity tableEntity, UserLog entity)
        {
            tableEntity["Action"] = new EntityProperty(entity.Action.ToString());
            tableEntity["Username"] = new EntityProperty(entity.Username);
            tableEntity["WhenOccurredUtc"] = new EntityProperty(entity.WhenOccurredUtc);
            tableEntity["AdditionalData"] = new EntityProperty(entity.AdditionalData);
        }

        protected override UserLog CreateEntityFromStorage(DynamicTableEntity tableEntity)
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
    }
}
