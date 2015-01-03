using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Queries
{
    public interface IAsyncQueryHandler<TParameter, TResult>
        where TResult : class
        where TParameter : class
    {
        Task<TResult> RetrieveAsync(TParameter query);
    }
}
