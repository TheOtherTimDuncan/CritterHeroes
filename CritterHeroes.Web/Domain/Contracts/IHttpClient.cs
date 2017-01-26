using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Domain.Contracts
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}
