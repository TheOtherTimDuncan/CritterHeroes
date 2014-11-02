using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AR.Dependency;
using AR.Domain.Contracts;
using AR.Domain.Handlers;
using AR.Domain.Models;
using AR.Domain.StateManagement;

namespace AR.Website.Controllers
{
    public class BaseController : Controller
    {
        private OrganizationContext _organizationContext;
        private IAppConfiguration _appConfiguration;

        public OrganizationContext OrganizationContext
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

        public IAppConfiguration AppConfiguration
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

        public T Using<T>()
        {
            return DependencyContainer.Using<T>();
        }

        public string GetBlobUrl(string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", AppConfiguration.BlobBaseUrl, OrganizationContext.AzureName.ToLower(), filename.ToLower());
        }
    }
}