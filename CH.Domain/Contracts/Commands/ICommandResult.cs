using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Commands
{
    public interface ICommandResult
    {
        bool Succeeded
        {
            get;
        }

        IDictionary<string, List<string>> Errors
        {
            get;
        }
    }
}
