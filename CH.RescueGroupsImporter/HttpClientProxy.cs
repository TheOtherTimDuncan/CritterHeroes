using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CritterHeroes.Web.Contracts;

namespace CH.RescueGroupsImporter
{
    public class HttpClientProxy : HttpClient, IHttpClient
    {
        public HttpClientProxy(Writer writer)
            : base(new LoggingHandler(new HttpClientHandler(), writer))
        {
        }
    }
}
