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

        private static readonly string[] _manifests = new[] { "/versioned-lib.json", "/versioned-js.json", "/versioned-css.json", "/versioned-templates.json" };

        public static bool IsDebug
        {
            get;
            set;
        }

        public static void Configure(IFileSystem fileSystem, IHttpContext httpContext)
        {
            _tags = new Dictionary<string, StaticFile>();

            IsDebug = httpContext.IsDebuggingEnabled;

            foreach (string manifest in _manifests)
            {
                string filename = fileSystem.MapServerPath(manifest);

                JObject json = JObject.Parse(fileSystem.ReadAllText(filename));

                foreach (JProperty property in json.Properties())
                {
                    string productionFilePath = property.Value.Value<string>();
                    string debugFilePath = property.Name.Replace(".min", "");
                    string debugUrl = httpContext.ConvertToAbsoluteUrl($"{pathDist}/{debugFilePath}");
                    string productionUrl = httpContext.ConvertToAbsoluteUrl($"{pathDist}/{productionFilePath}");
                    _tags[fileSystem.GetFileName(debugFilePath)] = new StaticFile(debugUrl, productionUrl);
                }
            }
        }

        public static string For(this UrlHelper urlHelper, string filename)
        {
            StaticFile staticFile;
            if (!_tags.TryGetValue(filename, out staticFile))
            {
                throw new InvalidOperationException($"No configuration found for {filename}");
            }

            string url = (IsDebug ? staticFile.DebugUrl : staticFile.ProductionUrl);
            return urlHelper.Content(url);
        }
    }
}
