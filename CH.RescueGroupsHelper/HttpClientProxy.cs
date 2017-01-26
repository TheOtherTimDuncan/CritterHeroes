using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CritterHeroes.Web.Domain.Contracts;

namespace CH.RescueGroupsHelper
{
    public class HttpClientProxy : HttpClient, IHttpClient
    {
        public HttpClientProxy(Writer writer, IList<string> responses = null)
            : base(new LoggingHandler(new HttpClientHandler(), writer, responses))
        {
        }
    }
}
