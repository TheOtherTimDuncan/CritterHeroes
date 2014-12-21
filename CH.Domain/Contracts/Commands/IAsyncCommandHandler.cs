using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface IAsyncCommandHandler<in TParameter, TResult>
        where TParameter : class
        where TResult : ICommandResult
    {
        Task<TResult> ExecuteAsync(TParameter command);
    }

    public interface IAsyncCommandHandler<in TParameter> : IAsyncCommandHandler<TParameter, CommandResult>
        where TParameter : class
    {
    }
}
