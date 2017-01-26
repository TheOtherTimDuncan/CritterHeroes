using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Domain.Contracts.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(IAsyncQuery<TResult> query);
        TResult Dispatch<TResult>(IQuery<TResult> query);

        Task RebuildAsync<TResult>(TResult queryResult) where TResult : class;
        void Rebuild<TResult>(TResult queryResult) where TResult : class;
    }
}
