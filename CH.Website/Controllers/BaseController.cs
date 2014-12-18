using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Domain.StateManagement;
using CH.Website.Middleware;
using Microsoft.AspNet.Identity;

namespace CH.Website.Controllers
{
    public class BaseController : Controller
    {
        private OrganizationContext _organizationContext;
        private UserContext _userContext;

        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public BaseController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        protected IQueryDispatcher QueryDispatcher
        {
            get
            {
                return _queryDispatcher;
            }
        }

        protected ICommandDispatcher CommandDispatcher
        {
            get
            {
                return _commandDispatcher;
            }
        }

        protected OrganizationContext OrganizationContext
        {
            get
            {
                if (_organizationContext == null)
                {
                    _organizationContext = Request.GetOwinContext().GetOrganizationContext();
                }
                return _organizationContext;
            }
        }

        protected UserContext UserContext
        {
            get
            {
                if (_userContext == null)
                {
                    _userContext = Request.GetOwinContext().GetUserContext();
                }
                return _userContext;
            }
        }

        protected void AddIdentityErrorsToModelState(ModelStateDictionary modelState, IdentityResult identityResult)
        {
            foreach (string error in identityResult.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected void AddCommandResultErrorsToModelState(ModelStateDictionary modelState, ICommandResult commandResult)
        {
            foreach (KeyValuePair<string, List<string>> error in commandResult.Errors)
            {
                foreach (string errorMessage in error.Value)
                {
                    modelState.AddModelError(error.Key, errorMessage);
                }
            }
        }
    }
}