using System;
using System.Collections.Generic;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandHandler<in TParameter>
        where TParameter : class
    {
        CommandResult Execute(TParameter command);
    }
}
