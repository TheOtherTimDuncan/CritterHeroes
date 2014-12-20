using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts.Queries;
using Ninject;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website.Services.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private IKernel _kernel;

        public QueryDispatcher(IKernel kernel)
        {
            ThrowIf.Argument.IsNull(kernel, "kernel");
            this._kernel = kernel;
        }

        public async Task<TResult> Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : class
            where TResult : class
        {
            return await _kernel.Get<IQueryHandler<TParameter, TResult>>().Retrieve(query);
        }
    }
}
