using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> Dispatch<TParameter>(TParameter command) where TParameter : ICommand;
    }
}
