using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandHandler<in TParameter> where TParameter : ICommand
    {
        Task<CommandResult> Execute(TParameter command);
    }
}
