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
            return await _kernel.Get<IAsyncCommandHandler<TParameter, CommandResult>>().ExecuteAsync(command);
        }

        public async Task<TResult> DispatchAsync<TParameter, TResult>(TParameter command)
            where TParameter : class
            where TResult : ICommandResult
        {
            return await _kernel.Get<IAsyncCommandHandler<TParameter, TResult>>().ExecuteAsync(command);
        }

        public CommandResult Dispatch<TParameter>(TParameter command) where TParameter : class
        {
            return _kernel.Get<ICommandHandler<TParameter, CommandResult>>().Execute(command);
        }

        public TResult Dispatch<TParameter, TResult>(TParameter command)
            where TParameter : class
            where TResult : ICommandResult
        {
            return _kernel.Get<ICommandHandler<TParameter, TResult>>().Execute(command);
        }
    }
}
