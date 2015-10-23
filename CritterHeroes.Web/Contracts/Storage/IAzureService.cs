using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IAzureService
    {
        string GetLoggingKey();
        string GetLoggingKeyForDate(DateTime logDateUtc);

        Task<CloudTable> GetCloudTable(string tableName);
        Task<CloudBlobContainer> GetBlobContainer();
    }
}
