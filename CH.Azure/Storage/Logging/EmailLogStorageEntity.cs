using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure.Utility;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CH.Azure.Storage.Logging
{
    public class EmailLogStorageEntity : StorageEntity<EmailLog>
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

        protected override void CopyEntityToStorage(DynamicTableEntity tableEntity, EmailLog entity)
        {
            tableEntity["ForUserID"] = new EntityProperty(entity.ForUserID);
            tableEntity["WhenSentUtc"] = new EntityProperty(entity.WhenSentUtc);
            tableEntity["Message"] = new EntityProperty(JsonConvert.SerializeObject(entity.Message));
        }

        protected override EmailLog CreateEntityFromStorage(DynamicTableEntity tableEntity)
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
    }
}
