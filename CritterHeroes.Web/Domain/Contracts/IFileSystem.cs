using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts
{
    public interface IFileSystem
    {
        string ReadAllText(string path);
        string CombinePath(params string[] paths);
        string GetFileName(string path);
        string GetFileNameWithoutExtension(string path);
        string GetFileExtension(string path);
        string MapServerPath(string path);
    }
}
