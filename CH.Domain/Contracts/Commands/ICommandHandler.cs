using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandHandler<in TParameter> where TParameter : class
    {
        Task<CommandResult> Execute(TParameter command);
    }
}
