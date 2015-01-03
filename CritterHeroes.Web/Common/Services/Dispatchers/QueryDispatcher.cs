using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using SimpleInjector;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Services.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private Container _container;

        public QueryDispatcher(Container container)
        {
            ThrowIf.Argument.IsNull(container, "kernel");
            this._container = container;
        }

        public async Task<TResult> DispatchAsync<TParameter, TResult>(TParameter query)
            where TParameter : class
            where TResult : class
        {
            return await _container.GetInstance<IAsyncQueryHandler<TParameter, TResult>>().RetrieveAsync(query);
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : class
            where TResult : class
        {
            return _container.GetInstance<IQueryHandler<TParameter, TResult>>().Retrieve(query);
        }
    }
}
