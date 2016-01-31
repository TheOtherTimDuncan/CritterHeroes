using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using TOTD.Utility.EnumerableHelpers;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public static class LoggerConfigurationAzureTableStorageExtensions
    {
        public static LoggerConfiguration AzureTableStorage(this LoggerSinkConfiguration loggerConfiguration, IAzureService azureService, string tableName)
        {
            AzureTableStorageSink sink = new AzureTableStorageSink(azureService, tableName);
            return loggerConfiguration.Sink(sink);
        }
    }

    public class AzureTableStorageSink : ILogEventSink
    {
        private IAzureService _azureService;
        private string _tableName = "log";

        public AzureTableStorageSink(IAzureService azureService, string tableName)
        {
            this._azureService = azureService;
            this._tableName = tableName;
        }

        public void Emit(LogEvent logEvent)
        {
            DynamicTableEntity tableEntity = new DynamicTableEntity(_azureService.GetLoggingKey(), Guid.NewGuid().ToString());
            tableEntity.Timestamp = logEvent.Timestamp.ToUniversalTime();
            tableEntity[nameof(LogEvent.Level)] = new EntityProperty(logEvent.Level.ToString());

            string message = logEvent.RenderMessage();
            tableEntity["Message"] = new EntityProperty(message);

            if (!logEvent.Properties.IsNullOrEmpty())
            {
                foreach (var property in logEvent.Properties)
                {
                    StringWriter writer = new StringWriter();
                    property.Value.Render(writer);
                    tableEntity[property.Key] = new EntityProperty(writer.ToString());
                }
            }

            if (logEvent.Exception != null)
            {
                tableEntity[nameof(LogEvent.Exception)] = new EntityProperty(logEvent.Exception.ToString());
            }

            TableOperation operation = TableOperation.InsertOrReplace(tableEntity);
            _azureService.ExecuteTableOperation(_tableName, operation);
        }
    }
}
