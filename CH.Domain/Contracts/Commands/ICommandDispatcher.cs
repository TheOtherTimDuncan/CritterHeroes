using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> Dispatch<TParameter>(TParameter command) 
            where TParameter : class;

        Task<TResult> Dispatch<TParameter, TResult>(TParameter command)
            where TParameter : class
            where TResult : ICommandResult;
    }
}
