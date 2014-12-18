using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandHandler<in TParameter, TResult>
        where TParameter : class
        where TResult : ICommandResult
    {
        Task<TResult> Execute(TParameter command);
    }

    public interface ICommandHandler<in TParameter> : ICommandHandler<TParameter, CommandResult>
        where TParameter : class
    {
    }
}
