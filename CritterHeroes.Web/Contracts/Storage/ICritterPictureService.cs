using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface ICritterPictureService
    {
        string GetPictureUrl(int critterID, string filename);
        Task GetPictureAsync(int critterID, string filename, Stream outputStream);
        Task SavePictureAsync(Stream source, int critterID, string filename, string contentType);
    }
}
