using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CH.Domain.Services.Commands;

namespace CH.Domain.Contracts.Commands
{
    public interface IAsyncCommandHandler<in TParameter>
        where TParameter : class
    {
        Task<CommandResult> ExecuteAsync(TParameter command);
    }
}
