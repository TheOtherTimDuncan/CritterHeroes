using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IAzureService
    {
        // Blobs

        string CreateBlobUrl(string path);
        Task<CloudBlockBlob> UploadBlobAsync(string path, bool isPrivate, string contentType, Stream source);
        Task<CloudBlockBlob> UploadBlobAsync(string path, bool isPrivate, string content);
        Task<string> DownloadBlobAsync(string path, bool isPrivate);
        Task DownloadBlobAsync(string path, bool isPrivate, Stream target);

        // Table Storage

        string GetLoggingKey();
        string GetLoggingKey(DateTime logDateUtc);
        Task<TableResult> ExecuteTableOperationAsync(string tableName, TableOperation operation);
        Task ExecuteTableBatchOperationAsync(string tableName, IEnumerable<ITableEntity> tableEntities, Func<ITableEntity, TableOperation> operation);
        Task<IQueryable<TElement>> CreateTableQuery<TElement>(string tableName) where TElement : ITableEntity, new();
    }
}
