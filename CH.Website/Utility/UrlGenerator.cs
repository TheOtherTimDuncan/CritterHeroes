using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts;
using TOTD.Mvc.Actions;

namespace CH.Website.Utility
{
    public class UrlGenerator : IUrlGenerator
    {
        private UrlHelper _urlHelper;

        public UrlGenerator(IHttpContext httpContext)
        {
            this._urlHelper = new UrlHelper(httpContext.Request.RequestContext);
        }

        public string GenerateAbsoluteUrl<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return GenerateAbsoluteUrl(ActionHelper.GetRouteValues<T>(actionSelector));
        }

        public string GenerateAbsoluteUrl<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            return GenerateAbsoluteUrl(ActionHelper.GetRouteValues<T>(actionSelector));
        }

        private string GenerateAbsoluteUrl(ActionHelperResult actionHelperResult)
        {
            return _urlHelper.Action(actionHelperResult.ActionName, actionHelperResult.ControllerName, actionHelperResult.RouteValues, _urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
        }

        public string GenerateSiteUrl<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return _urlHelper.Action<T>(actionSelector);
        }

        public string GenerateSiteUrl<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            return _urlHelper.Action<T>(actionSelector);
        }
    }
}