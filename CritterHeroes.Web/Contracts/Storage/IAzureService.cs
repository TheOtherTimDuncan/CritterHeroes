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

        Task<CloudBlockBlob> UploadBlobAsync(string path, bool isPrivate, string contentType, Stream source);
        Task<CloudBlockBlob> UploadBlobAsync(string path, bool isPrivate, string content);
        Task<string> DownloadBlobAsync(string path, bool isPrivate);
        Task DownloadBlobAsync(string path, bool isPrivate, Stream target);
    }
}
