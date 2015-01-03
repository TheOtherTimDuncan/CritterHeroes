using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Queries
{
    public interface IAsyncQueryHandler<in TParameter, TResult>
        where TParameter : IAsyncQuery<TResult>
    {
        Task<TResult> RetrieveAsync(TParameter query);
    }
}
