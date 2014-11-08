using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CH.Domain.Contracts;
using TOTD.Utility.ExceptionHelpers;

namespace CH.Domain.Proxies
{
    public class HttpContextProxy : IHttpContext
    {
        private HttpContextBase _httpContext;

        public HttpContextProxy()
        {
            ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
            _httpContext = new HttpContextWrapper(HttpContext.Current);
        }

        public HttpRequestBase Request
        {
            get
            {
                return _httpContext.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                return _httpContext.Response;
            }
        }

        public IPrincipal User
        {
            get
            {
                return _httpContext.User;
            }
        }

        public HttpServerUtilityBase Server
        {
            get
            {
                return _httpContext.Server;
            }
        }
    }
}
