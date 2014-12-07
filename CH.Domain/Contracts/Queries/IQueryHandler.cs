using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CH.Domain.Contracts.Queries
{
    public interface IQueryHandler<TParameter, TResult>
        where TResult : class
        where TParameter : class
    {
        Task<TResult> Retrieve(TParameter query);
    }
}
