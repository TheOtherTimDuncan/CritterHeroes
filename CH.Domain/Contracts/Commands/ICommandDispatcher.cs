using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> DispatchAsync<TParameter>(TParameter command)
            where TParameter : class;

        CommandResult Dispatch<TParameter>(TParameter command)
        where TParameter : class;
    }
}
