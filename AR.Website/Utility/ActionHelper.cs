using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace AR.Website.Utility
{
    public static class ActionHelper
    {
        private const string controllerSuffix = "Controller";

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            return GetRouteValues<T>(actionSelector, null);
        }

        public static ActionHelperResult GetRouteValues<T>(Expression<Func<T, ActionResult>> actionSelector, RouteValueDictionary routeValues) where T : IController
        {
            ActionHelperResult result = new ActionHelperResult()
            {
                RouteValues = routeValues ?? new RouteValueDictionary()
            };

            Type controllerType = typeof(T);

            // Only set the area if it is not already there
            if (!result.RouteValues.ContainsKey("area"))
            {
                // Get the area from the controller if it has the attribute
                RouteAreaAttribute controllerArea = controllerType.GetCustomAttribute<RouteAreaAttribute>();
                if (controllerArea != null)
                {
                    result.RouteValues.Add("area", controllerArea.AreaName);
                }
                else
                {
                    // Default area for non-area based actions is an empty string
                    result.RouteValues.Add("area", string.Empty);
                }
            }

            MethodCallExpression methodExpression = actionSelector.Body as MethodCallExpression;

            if (methodExpression == null || methodExpression.Object.Type != controllerType)
            {
                throw new ArgumentException("You must call a method of " + controllerType.Name, "actionSelector");
            }

            // Controller name is name of controller class with Controller removed from the end if it is there
            if (controllerType.Name.EndsWith(controllerSuffix))
            {
                result.ControllerName = controllerType.Name.Substring(0, controllerType.Name.Length - controllerSuffix.Length);
            }
            else
            {
                result.ControllerName = controllerType.Name;
            }

            // Action name is the name of the method being called
            result.ActionName = methodExpression.Method.Name;

            int i = 0;
            ParameterInfo[] parameters = methodExpression.Method.GetParameters();
            foreach (Expression arg in methodExpression.Arguments)
            {
                string parameterName = parameters[i].Name;
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