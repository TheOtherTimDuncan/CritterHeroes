﻿using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Queries
{
    public interface IQueryHandler<TParameter, TResult>
        where TResult : class
        where TParameter : class
    {
        TResult Retrieve(TParameter query);
    }
}
