using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using CH.Domain.Contracts;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Proxies
{
    public class HttpContextProxy : IHttpContext
    {
        private HttpContextBase _httpContext;
        
        // We need to late-bind to HttpContext rather than do it in the
        private HttpContextBase Context
        {
            get
            {
                if (_httpContext == null)
                {
                    ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                    _httpContext = new HttpContextWrapper(HttpContext.Current);
                }
                return _httpContext;
            }
        }

        public HttpRequestBase Request
        {
            get
            {
                return Context.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                return Context.Response;
            }
        }

        public IPrincipal User
        {
            get
            {
                return Context.User;
            }
        }

        public HttpServerUtilityBase Server
        {
            get
            {
                return Context.Server;
            }
        }
    }
}
