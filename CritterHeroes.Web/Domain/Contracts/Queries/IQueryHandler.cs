using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Domain.Contracts.Queries
{
    public interface IQueryHandler<in TParameter, out TResult> where TParameter : IQuery<TResult>
    {
        TResult Execute(TParameter query);
    }

    public interface IAsyncQueryHandler<in TParameter, TResult> where TParameter : IAsyncQuery<TResult>
    {
        Task<TResult> ExecuteAsync(TParameter query);
    }
}
