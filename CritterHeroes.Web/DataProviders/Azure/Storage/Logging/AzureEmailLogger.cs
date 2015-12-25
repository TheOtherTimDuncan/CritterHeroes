using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Models.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureEmailLogger : BaseAzureLoggerStorageContext<EmailLog>, IEmailLogger
    {
        private IAzureService _azureService;

        private const string _blobPath = "email";
        private const bool _isPrivate = true;
        private const string _tableName = "emaillog";

        public AzureEmailLogger(IAzureService azureService)
            : base(_tableName, azureService)
        {
            this._azureService = azureService;
        }

        public async Task<IEnumerable<EmailLog>> GetEmailLogAsync(DateTime dateFrom, DateTime dateTo)
        {
            string start = _azureService.GetLoggingKey(dateFrom);
            string end = _azureService.GetLoggingKey(dateTo);

            var entities =
                (
                    from e in await _azureService.CreateTableQuery<DynamicTableEntity>(_tableName)
                    where e.PartitionKey.CompareTo(start) >= 0 && e.PartitionKey.CompareTo(end) <= 0
                    select e
                ).ToList();

            List<EmailLog> result = new List<EmailLog>();
            foreach (DynamicTableEntity tableEntity in entities)
            {
                EmailLog emailLog = FromStorage(tableEntity);

                emailLog.EmailData = await _azureService.DownloadBlobAsync($"{_blobPath}/{emailLog.ID}", _isPrivate);

                result.Add(emailLog);
            }
            return result;
        }

        public async Task LogEmailAsync(EmailLog emailLog)
        {
            await SaveAsync(emailLog);
            await _azureService.UploadBlobAsync($"{_blobPath}/{emailLog.ID}", _isPrivate, emailLog.EmailData);
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
