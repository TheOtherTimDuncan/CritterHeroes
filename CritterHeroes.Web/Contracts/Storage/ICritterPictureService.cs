using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface ICritterPictureService
    {
        Task<bool> PictureExistsAsync(int critterID, string filename);
        string GetPictureUrl(int critterID, string filename);
        string GetNotFoundUrl();
        Task GetPictureAsync(int critterID, string filename, Stream outputStream);
        Task SavePictureAsync(Stream source, int critterID, string filename, string contentType);
    }
}
