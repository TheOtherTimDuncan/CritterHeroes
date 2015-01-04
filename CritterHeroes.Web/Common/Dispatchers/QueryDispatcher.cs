﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using SimpleInjector;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private Container _container;

        public QueryDispatcher(Container container)
        {
            ThrowIf.Argument.IsNull(container, "kernel");
            this._container = container;
        }

        public async Task<TResult> DispatchAsync<TResult>(IAsyncQuery<TResult> query)
        {
            Type handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _container.GetInstance(handlerType);
            return await handler.RetrieveAsync((dynamic)query);
        }

        public TResult Dispatch<TResult>(IQuery<TResult> query)
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _container.GetInstance(handlerType);
            return handler.Retrieve((dynamic)query);
        }
    }
}