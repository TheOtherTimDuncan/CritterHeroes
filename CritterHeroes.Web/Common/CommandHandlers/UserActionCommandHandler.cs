using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.CommandHandlers
{
    public class UserActionCommandHandler<T> : IAsyncCommandHandler<T>
        where T:class,IUserCommand
    {
        private IAsyncUserCommandHandler<T> _handler;
        private IUserLogger _userLogger;

        public UserActionCommandHandler(IAsyncCommandHandler<T> handler, IUserLogger userLogger)
        {
            this._userLogger = userLogger;

            this._handler = handler as IAsyncUserCommandHandler<T>;
            ThrowIf.Argument.IsNull(this._handler, "handler is not of type IAsyncCommandHandler");
        }

        public async Task<CommandResult> ExecuteAsync(T command)
        {
            CommandResult commandResult = await _handler.ExecuteAsync(command);

            if (commandResult.Succeeded)
            {
                await _userLogger.LogActionAsync(_handler.SuccessUserAction, command.Username);
            }
            else
            {
                await _userLogger.LogActionAsync(_handler.FailedUserAction, command.Username, command);
            }

            return commandResult;
        }
    }
}