using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Domain.Contracts.Queries
{
    public interface IQueryRebuilder<TResult> where TResult : class
    {
        void Rebuild(TResult queryResult);
    }

    public interface IAsyncQueryRebuilder<TResult> where TResult : class
    {
        Task RebuildAsync(TResult queryResult);
    }
}
