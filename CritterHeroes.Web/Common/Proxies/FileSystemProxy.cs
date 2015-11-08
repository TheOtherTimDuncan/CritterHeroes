using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Contracts;

namespace CritterHeroes.Web.Common.Proxies
{
    public class FileSystemProxy : IFileSystem
    {
        private IHttpContext _httpContext;

        public FileSystemProxy(IHttpContext httpContext)
        {
            this._httpContext = httpContext;
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public string MapServerPath(string path)
        {
            return _httpContext.Server.MapPath($"~/{path}");
        }
    }
}
