using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Queries;

namespace CH.Website.Services.QueryHandlers
{
    public abstract class BaseViewModelQueryHandler<TParameter, TResult> : IAsyncQueryHandler<TParameter, TResult>
        where TResult : class
        where TParameter : class
    {
        public abstract Task<TResult> RetrieveAsync(TParameter query);

        protected string GetBlobUrl(string baseBlobUrl, string azurename, string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", baseBlobUrl, azurename.ToLower(), filename.ToLower());
        }
    }
}