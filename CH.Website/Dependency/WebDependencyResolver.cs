using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;

namespace CH.Website.Dependency
{
    public class WebDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public WebDependencyResolver(IKernel kernel)
        {
            this._kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = _kernel.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _kernel.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = _kernel.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _kernel.Resolve(request);
        }
    }
}
