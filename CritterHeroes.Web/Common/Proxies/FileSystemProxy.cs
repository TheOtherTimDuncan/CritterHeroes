using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Contracts;

namespace CritterHeroes.Web.Common.Proxies
{
    public class FileSystemProxy : IFileSystem
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
