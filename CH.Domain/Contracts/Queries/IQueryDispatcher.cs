using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CH.Domain.Contracts.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : class
            where TResult : class;
    }
}
