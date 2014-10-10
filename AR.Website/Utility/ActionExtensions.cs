using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace AR.Website.Utility
{
    public static class ActionExtensions
    {
        public static string Action<T>(this UrlHelper urlHelper, Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return urlHelper.Action<T>(actionSelector, null);
        }

        public static string Action<T>(this UrlHelper urlHelper, Expression<Func<T, ActionResult>> actionSelector, object routeValues) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<T>(actionSelector, routeValues);
            return urlHelper.Action(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
        }

        public static string Action<T>(this UrlHelper urlHelper, Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            return urlHelper.Action<T>(actionSelector, null);
        }

        public static string Action<T>(this UrlHelper urlHelper, Expression<Func<T, Task<ActionResult>>> actionSelector, object routeValues) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<T>(actionSelector, routeValues);
            return urlHelper.Action(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
        }

        public static void RenderAction<T>(this HtmlHelper htmlHelper, Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            htmlHelper.RenderAction<T>(actionSelector, null);
        }

        public static void RenderAction<T>(this HtmlHelper htmlHelper, Expression<Func<T, ActionResult>> actionSelector, object routeValues) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<T>(actionSelector, routeValues);
            htmlHelper.RenderAction(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
        }

        public static void RenderAction<T>(this HtmlHelper htmlHelper, Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            htmlHelper.RenderAction<T>(actionSelector, null);
        }

        public static void RenderAction<T>(this HtmlHelper htmlHelper, Expression<Func<T, Task<ActionResult>>> actionSelector, object routeValues) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<T>(actionSelector, routeValues);
            htmlHelper.RenderAction(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
        }
    }
}