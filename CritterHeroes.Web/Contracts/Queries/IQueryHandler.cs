using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Queries
{
    public interface IQueryHandler<in TParameter, out TResult>
        where TParameter : IQuery<TResult>
    {
        TResult Retrieve(TParameter query);
    }
}
