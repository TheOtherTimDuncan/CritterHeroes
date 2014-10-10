using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace AR.Website.Utility
{
    public static class ActionHelper
    {
        private const string controllerSuffix = "Controller";

        private static ConcurrentDictionary<Type, ControllerContext> cache = new ConcurrentDictionary<Type, ControllerContext>();

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return GetRouteValues<T>(actionSelector, null);
        }

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, ActionResult>> actionSelector, object routeValues) where T : IController
        {
            RouteValueDictionary dictionaryValues = new RouteValueDictionary(routeValues);
            return GetRouteValues<T>(actionSelector, dictionaryValues);
        }

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            return GetRouteValues<T>((actionSelector.Body as MethodCallExpression), null);
        }

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, Task<ActionResult>>> actionSelector, object routeValues) where T : IController
        {
            RouteValueDictionary dictionaryValues = new RouteValueDictionary(routeValues);
            return GetRouteValues<T>((actionSelector.Body as MethodCallExpression), dictionaryValues);
        }

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, ActionResult>> actionSelector, RouteValueDictionary routeValues) where T : IController
        {
            return GetRouteValues<T>((actionSelector.Body as MethodCallExpression), routeValues);
        }

        public static ActionHelperResult GetRouteValues<T>(MethodCallExpression methodExpression, RouteValueDictionary routeValues) where T : IController
        {
            ActionHelperResult result = new ActionHelperResult()
            {
                RouteValues = routeValues ?? new RouteValueDictionary()
            };

            Type controllerType = typeof(T);

            if (methodExpression == null || methodExpression.Object.Type != controllerType)
            {
                throw new ArgumentException("You must call a method of " + controllerType.Name, "actionSelector");
            }

            string areaName = string.Empty;  // Default area for non-area based actions is an empty string

            // Check for cached controller context
            ControllerContext controllerContext;
            if (cache.TryGetValue(controllerType, out controllerContext))
            {
                // Use cached values
                result.ControllerName = controllerContext.ControllerName;
                areaName = controllerContext.AreaName;
            }
            else
            {
                // Controller name is name of controller class with Controller removed from the end if it is there
                if (controllerType.Name.EndsWith(controllerSuffix))
                {
                    result.ControllerName = controllerType.Name.Substring(0, controllerType.Name.Length - controllerSuffix.Length);
                }
                else
                {
                    result.ControllerName = controllerType.Name;
                }

                // Get the area from the controller if it has the attribute
                RouteAreaAttribute controllerArea = controllerType.GetCustomAttribute<RouteAreaAttribute>();
                if (controllerArea != null)
                {
                    areaName = controllerArea.AreaName;
                }

                // Cache the results
                controllerContext = new ControllerContext(result.ControllerName, areaName);
                cache.TryAdd(controllerType, controllerContext);
            }

            // Only set the area if it is not already there
            if (!result.RouteValues.ContainsKey("area"))
            {
                result.RouteValues.Add("area", areaName);
            }

            // Action name is the name of the method being called
            result.ActionName = methodExpression.Method.Name;

            // Check for cached action 
            string[] parameterNames;
            if (!controllerContext.TryGetActionParameterNames(result.ActionName, out parameterNames))
            {
                parameterNames = methodExpression.Method.GetParameters().Select(x => x.Name).ToArray();

                // Cache parameter names
                controllerContext.AddAction(result.ActionName, parameterNames);
            }

            int i = 0;
            foreach (Expression arg in methodExpression.Arguments)
            {
                string parameterName = parameterNames[i];
                if (!result.RouteValues.ContainsKey(parameterName))
                {
                    object parameterValue;
                    if (arg.NodeType == ExpressionType.Constant)
                    {
                        parameterValue = ((ConstantExpression)arg).Value;
                    }
                    else
                    {
                        parameterValue = Expression.Lambda(arg).Compile().DynamicInvoke(null);
                    }
                    if (parameterValue != null)
                    {
                        result.RouteValues.Add(parameterName, parameterValue);
                    }
                }
            }

            return result;
        }

        private class ControllerContext
        {
            private ConcurrentDictionary<string, string[]> _actions;

            public ControllerContext(string controllerName, string areaName)
            {
                this.ControllerName = controllerName;
                this.AreaName = areaName;

                this._actions = new ConcurrentDictionary<string, string[]>();
            }

            public string ControllerName
            {
                get;
                private set;
            }

            public string AreaName
            {
                get;
                private set;
            }

            public bool TryGetActionParameterNames(string actionName, out string[] parameterNames)
            {
                return _actions.TryGetValue(actionName, out parameterNames);
            }

            public void AddAction(string actionName, string[] parameterNames)
            {
                _actions.TryAdd(actionName, parameterNames);
            }
        }
    }

    public class ActionHelperResult
    {
        public string ControllerName
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

        public RouteValueDictionary RouteValues
        {
            get;
            set;
        }
    }
}