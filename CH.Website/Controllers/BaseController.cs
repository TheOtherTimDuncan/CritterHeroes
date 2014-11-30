using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CH.Dependency;
using CH.Domain.Commands;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Handlers;
using CH.Domain.Models;
using CH.Domain.StateManagement;
using Microsoft.AspNet.Identity;

namespace CH.Website.Controllers
{
    public class BaseController : Controller
    {
        private OrganizationContext _organizationContext;
        private IAppConfiguration _appConfiguration;

        protected OrganizationContext OrganizationContext
        {
            get
            {
                if (_organizationContext == null)
                {
                    _organizationContext = Using<IStateManager<OrganizationContext>>().GetContext();
                    if (_organizationContext == null)
                    {
                        Organization organization = Task.Factory.StartNew(() => Using<IStorageContext<Organization>>().GetAsync(AppConfiguration.OrganizationID.ToString()).Result, TaskCreationOptions.LongRunning).Result;
                        _organizationContext = OrganizationContext.FromOrganization(organization);
                        Using<IStateManager<OrganizationContext>>().SaveContext(_organizationContext);
                    }
                }
                return _organizationContext;
            }
        }

        protected IAppConfiguration AppConfiguration
        {
            get
            {
                if (_appConfiguration == null)
                {
                    _appConfiguration = DependencyContainer.Using<IAppConfiguration>();
                }
                return _appConfiguration;
            }
        }

        protected T Using<T>()
        {
            return DependencyContainer.Using<T>();
        }

        protected string GetBlobUrl(string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", AppConfiguration.BlobBaseUrl, OrganizationContext.AzureName.ToLower(), filename.ToLower());
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
            foreach (KeyValuePair<string, string> error in commandResult.Errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }
        }
    }
}