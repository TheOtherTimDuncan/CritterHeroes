using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure.Storage;
using CH.Azure.Utility;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace CH.Azure.Logging
{
    public class AzureUserLogger : AzureStorage<UserLog>, IUserLogger
    {
        private const string _tableName = "errorlog";

        public AzureUserLogger(IAzureConfiguration azureConfiguration)
            : base("userlog", azureConfiguration)
        {
        }

        public async Task<IEnumerable<UserLog>> GetUserLog(DateTime dateFrom, DateTime dateTo)
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

            StorageEntity<UserLog> storageEntity = StorageEntityFactory.GetStorageEntity<UserLog>();

            List<UserLog> result = new List<UserLog>();
            foreach (DynamicTableEntity tableEntity in entities)
            {
                storageEntity.TableEntity = tableEntity;
                result.Add(storageEntity.Entity);
            }
            return result;
        }

        public async Task LogAction(UserActions userAction, string userName)
        {
            UserLog userLog = new UserLog(userAction, userName, DateTime.UtcNow);
            await SaveAsync(userLog);
        }

        public async Task LogAction<T>(UserActions userAction, string userName, T additionalData)
        {
            UserLog userLog = new UserLog(userAction, userName, DateTime.UtcNow);
            userLog.AdditionalData = JsonConvert.SerializeObject(additionalData);
            await SaveAsync(userLog);
        }
    }
}
