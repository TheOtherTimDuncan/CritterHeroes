using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Services.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Areas.Account.Handlers
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

        public CommandResult Execute(LogoutModel command)
        {
            _authenticationManager.SignOut();
            _stateManager.ClearContext();

            return CommandResult.Success();
        }
    }
}