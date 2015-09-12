using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CritterHeroes.Web.Contracts
{
    public interface IUrlGenerator
    {
        string GenerateAbsoluteUrl<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController;
        string GenerateAbsoluteUrl<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController;
        string GenerateAbsoluteUrl(string actionName, string controllerName) ;
        string GenerateAbsoluteUrl(string actionName, string controllerName, object routeValues) ;

        string GenerateSiteUrl<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController;
        string GenerateSiteUrl<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController;
        string GenerateSiteUrl(string actionName, string controllerName);
        string GenerateSiteUrl(string actionName, string controllerName, object routeValues);
    }
}
