using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.Common.VersionedStatics
{
    public static class VersionedStatics
    {
        private static Dictionary<string, StaticFile> _tags;

        private const string pathDist = "~/dist";

        private static readonly string[,] _manifests = new string[,] {
            { "/versioned-lib.json","js" },
            { "/versioned-js.json","js" },
            { "/versioned-css.json","css" }
        };

        public static bool IsDebug
        {
            get;
            set;
        }

        public static void Configure(IFileSystem fileSystem, IHttpContext httpContext)
        {
            _tags = new Dictionary<string, StaticFile>();

            IsDebug = httpContext.IsDebuggingEnabled;

            for (int i = 0; i < _manifests.GetLength(0); i++)
            {
                string filename = fileSystem.MapServerPath(_manifests[i, 0]);
                string folder = _manifests[i, 1];

                JObject json = JObject.Parse(fileSystem.ReadAllText(filename));

                foreach (JProperty property in json.Properties())
                {
                    string debugFilename = property.Value.Value<string>();
                    string productionFilename = fileSystem.GetFileNameWithoutExtension(debugFilename) + ".min" + fileSystem.GetFileExtension(debugFilename);
                    string debugUrl = httpContext.ConvertToAbsoluteUrl($"{pathDist}/{folder}/{debugFilename}");
                    string productionUrl = httpContext.ConvertToAbsoluteUrl($"{pathDist}/{folder}/{productionFilename}");
                    _tags[property.Name] = new StaticFile(debugUrl, productionUrl);
                }
            }
        }

        public static string UrlFor(string filename)
        {
            StaticFile staticFile;
            if (!_tags.TryGetValue(filename, out staticFile))
            {
                throw new InvalidOperationException($"No configuration found for {filename}");
            }

            string url = (IsDebug ? staticFile.DebugUrl : staticFile.ProductionUrl);
#if DEBUG
            IHttpContext httpContext = DependencyResolver.Current.GetService<IHttpContext>();
            IFileSystem fileSystem = DependencyResolver.Current.GetService<IFileSystem>();
            string localFilename = httpContext.Server.MapPath(url);
            if (!System.IO.File.Exists(localFilename))
            {
                Configure(fileSystem, httpContext);
                return UrlFor(filename);
            }
#endif
            return url;
        }
    }
}
