using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CH.Dependency;
using CH.Domain.Commands;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Queries;
using CH.Domain.Handlers;
using CH.Domain.Models;
using CH.Domain.Queries;
using CH.Domain.StateManagement;
using Microsoft.AspNet.Identity;

namespace CH.Website.Controllers
{
    public class BaseController : Controller
    {
        private OrganizationContext _organizationContext;
        private IAppConfiguration _appConfiguration;

        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public BaseController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IAppConfiguration appConfiguration)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
            this._appConfiguration = appConfiguration;
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

        protected OrganizationContext GetOrganizationContext()
        {
            if (_organizationContext == null)
            {
                // First try to get the context from the state manager
                IStateManager<OrganizationContext> stateManager = Using<IStateManager<OrganizationContext>>();
                _organizationContext = stateManager.GetContext();

                if (_organizationContext == null)
                {
                    // State manager must not have it yet or it's been lost so let's create from scratch

                    Organization organization = Task.Run(() =>
                    {
                        return QueryDispatcher.Dispatch<OrganizationQuery, Organization>(new OrganizationQuery()
                        {
                            OrganizationID = _appConfiguration.OrganizationID
                        });
                    }).Result;

                    _organizationContext = OrganizationContext.FromOrganization(organization);
                    stateManager.SaveContext(_organizationContext);
                }
            }
            return _organizationContext;
        }

        protected T Using<T>()
        {
            return DependencyContainer.Using<T>();
        }

        protected string GetBlobUrl(string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", _appConfiguration.BlobBaseUrl, GetOrganizationContext().AzureName.ToLower(), filename.ToLower());
        }

        protected void AddIdentityErrorsToModelState(ModelStateDictionary modelState, IdentityResult identityResult)
        {
            foreach (string error in identityResult.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected void AddCommandResultErrorsToModelState(ModelStateDictionary modelState, CommandResult commandResult)
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