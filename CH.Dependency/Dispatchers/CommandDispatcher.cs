using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;
using CH.Domain.Contracts.Commands;
using Ninject;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Dependency.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private IKernel _kernel;

        public CommandDispatcher(IKernel kernel)
        {
            ThrowIf.Argument.IsNull(kernel, "kernel");
            this._kernel = kernel;
        }

        public async Task<CommandResult> Dispatch<TParameter>(TParameter command) where TParameter : class
        {
            return await _kernel.Get<ICommandHandler<TParameter, CommandResult>>().Execute(command);
        }

        public async Task<TResult> Dispatch<TParameter, TResult>(TParameter command)
            where TParameter : class
            where TResult : ICommandResult
        {
            return await _kernel.Get<ICommandHandler<TParameter, TResult>>().Execute(command);
        }
    }
}
