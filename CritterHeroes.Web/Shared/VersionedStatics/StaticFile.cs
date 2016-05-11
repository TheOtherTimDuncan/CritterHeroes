using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared.VersionedStatics
{
    public class StaticFile
    {
        public StaticFile(string debugUrl, string productionUrl)
        {
            this.DebugUrl = debugUrl;
            this.ProductionUrl = productionUrl;
        }

        public string DebugUrl
        {
            get;
        }

        public string ProductionUrl
        {
            get;
        }
    }
}
