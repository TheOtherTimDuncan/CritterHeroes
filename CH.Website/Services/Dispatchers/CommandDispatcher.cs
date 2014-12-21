using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Commands;
using CH.Domain.Services.Commands;
using Ninject;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Website.Services.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private IKernel _kernel;

        public CommandDispatcher(IKernel kernel)
        {
            ThrowIf.Argument.IsNull(kernel, "kernel");
            this._kernel = kernel;
        }

        public async Task<CommandResult> DispatchAsync<TParameter>(TParameter command) where TParameter : class
        {
            return await _kernel.Get<IAsyncCommandHandler<TParameter>>().ExecuteAsync(command);
        }

        public CommandResult Dispatch<TParameter>(TParameter command) where TParameter : class
        {
            return _kernel.Get<ICommandHandler<TParameter>>().Execute(command);
        }
    }
}
