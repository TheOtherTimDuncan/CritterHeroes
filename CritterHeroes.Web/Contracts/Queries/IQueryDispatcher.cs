using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TParameter, TResult>(TParameter query)
            where TParameter : class
            where TResult : class;

        TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : class
            where TResult : class;
    }
}
