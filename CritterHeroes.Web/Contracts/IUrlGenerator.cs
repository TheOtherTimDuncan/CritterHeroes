using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts
{
    public interface IUrlGenerator
    {
        string GenerateAbsoluteUrl(string actionName, string controllerName);
        string GenerateAbsoluteUrl(string actionName, string controllerName, object routeValues);

        string GenerateSiteUrl(string actionName, string controllerName);
        string GenerateSiteUrl(string actionName, string controllerName, object routeValues);
    }
}
