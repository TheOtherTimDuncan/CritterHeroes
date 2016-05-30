using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace CritterHeroes.Web.Contracts
{
    public interface IUrlGenerator
    {
        string GenerateAbsoluteUrl(string actionName, string controllerName);
        string GenerateAbsoluteUrl(string actionName, string controllerName, object routeValues);
        string GenerateAbsoluteUrl(string actionName, string controllerName, RouteValueDictionary routeValues);

        string GenerateSiteUrl(string actionName, string controllerName);
        string GenerateSiteUrl(string actionName, string controllerName, object routeValues);
        string GenerateSiteUrl(string actionName, string controllerName, RouteValueDictionary routeValues);
    }
}
