using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Features.Common
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

        public string GenerateAbsoluteUrl(string actionName, string controllerName)
        {
            return GenerateAbsoluteUrl(actionName, controllerName, null);
        }

        public string GenerateAbsoluteUrl(string actionName, string controllerName, object routeValues)
        {
            return Url.Action(actionName, controllerName, routeValues, _urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
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
