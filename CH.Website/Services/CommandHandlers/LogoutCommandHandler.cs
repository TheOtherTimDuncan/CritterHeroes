using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Services.Commands;
using CH.Domain.StateManagement;
using CH.Website.Models;
using Microsoft.Owin.Security;

namespace CH.Website.Services.CommandHandlers
{
    public class LogoutCommandHandler : ICommandHandler<LogoutModel>
    {
        private IAuthenticationManager _authenticationManager;
        private IStateManager<UserContext> _stateManager;

        public LogoutCommandHandler(IAuthenticationManager authenticationManager, IStateManager<UserContext> stateManager)
        {
            this._authenticationManager = authenticationManager;
            this._stateManager = stateManager;
        }

        public Task<CommandResult> Execute(LogoutModel command)
        {
            _authenticationManager.SignOut();
            _stateManager.ClearContext();

            return Task.FromResult(CommandResult.Success());
        }
    }
}