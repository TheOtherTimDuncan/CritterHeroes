using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts;
using TOTD.Mvc.Actions;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Areas.Common
{
    public class UrlGenerator : IUrlGenerator
    {
        private UrlHelper _urlHelper;

        private UrlHelper Url
        {
            get
            {
                if (_urlHelper == null)
                {
                    ThrowIf.Argument.IsNull(HttpContext.Current, "HttpContext.Current");
                    _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                }
                return _urlHelper;
            }
        }

        public string GenerateAbsoluteUrl<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return GenerateAbsoluteUrl(ActionHelper.GetRouteValues<T>(actionSelector));
        }

        public string GenerateAbsoluteUrl<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            return GenerateAbsoluteUrl(ActionHelper.GetRouteValues<T>(actionSelector));
        }

        public string GenerateAbsoluteUrl(string actionName, string controllerName)
        {
            return GenerateAbsoluteUrl(actionName, controllerName, null);
        }

        public string GenerateAbsoluteUrl(string actionName, string controllerName, object routeValues)
        {
            return Url.Action(actionName, controllerName, routeValues, _urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
        }

        private string GenerateAbsoluteUrl(ActionHelperResult actionHelperResult)
        {
            return Url.Action(actionHelperResult.ActionName, actionHelperResult.ControllerName, actionHelperResult.RouteValues, _urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
        }

        public string GenerateSiteUrl<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return Url.Action<T>(actionSelector);
        }

        public string GenerateSiteUrl<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            return Url.Action<T>(actionSelector);
        }

        public string GenerateSiteUrl(string actionName, string controllerName)
        {
            return GenerateSiteUrl(actionName, controllerName, null);
        }

        public string GenerateSiteUrl(string actionName, string controllerName, object routeValues)
        {
            return Url.Action(actionName, controllerName, routeValues);
        }
    }
}
