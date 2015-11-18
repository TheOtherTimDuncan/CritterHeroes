using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.Common.VersionedStatics
{
    public static class VersionedStatics
    {
        private static Dictionary<string, StaticFile> _tags;

        private const string pathDist = "~/dist";

        public static bool IsDebug
        {
            get;
            set;
        }

        public static void Configure(IFileSystem fileSystem, IHttpContext httpContext)
        {
            _tags = new Dictionary<string, StaticFile>();

            IsDebug = httpContext.IsDebuggingEnabled;

            string libFilename = fileSystem.MapServerPath("/versioned-lib.json");
            JObject json = JObject.Parse(fileSystem.ReadAllText(libFilename));

            foreach (JProperty property in json.Properties())
            {
                string debugFilename = property.Value.Value<string>();
                string productionFilename = fileSystem.GetFileNameWithoutExtension(debugFilename) + ".min" + fileSystem.GetFileExtension(debugFilename);
                string debugUrl = httpContext.ConvertToAbsoluteUrl($"{pathDist}/js/{debugFilename}");
                string productionUrl = httpContext.ConvertToAbsoluteUrl($"{pathDist}/js/{productionFilename}");
                _tags[property.Name] = new StaticFile(debugUrl, productionUrl);
            }
        }

        public static string UrlFor(string filename)
        {
            StaticFile staticFile;
            if (!_tags.TryGetValue(filename, out staticFile))
            {
                throw new InvalidOperationException($"No configuration found for {filename}");
            }

            return (IsDebug ? staticFile.DebugUrl : staticFile.ProductionUrl);
        }
    }
}
