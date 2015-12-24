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

        Task<CloudBlockBlob> UploadBlobAsync(string path,string contentType, Stream source);
        Task<CloudBlockBlob> UploadBlobAsync(string path, string content);
        Task<string> DownloadBlobAsync(string path);
        Task DownloadBlobAsync(string path, Stream target);
    }
}
