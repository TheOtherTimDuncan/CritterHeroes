using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CritterHeroes.Web.Contracts;

namespace CritterHeroes.Web.Common.Proxies
{
    public class HttpClientProxy : HttpClient, IHttpClient
    {
    }
}
