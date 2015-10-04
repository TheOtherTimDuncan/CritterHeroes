using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using CritterHeroes.Web.Areas.Common.Queries;
using CritterHeroes.Web.Middleware;
using TOTD.Mvc.FluentHtml;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class MiscExtensions
    {
        public static string Picture(this UrlHelper urlHelper, int critterID, string filename, int? desiredWidth = null, int? desiredHeight = null)
        {
            StringBuilder builder = new StringBuilder();

            if (desiredWidth != null)
            {
                builder.Append("width=");
                builder.Append(desiredWidth);
            }

            if (desiredHeight != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&");
                }
                builder.Append("height=");
                builder.Append(desiredHeight);
            }

            if (builder.Length > 0)
            {
                builder.Insert(0, "?");
            }

            return urlHelper.Content($"~/{ImageMiddleware.Route}/{critterID}/{filename}{builder}");
        }

        public static string Page(this UrlHelper urlHelper, int page, PagingQuery originalQuery)
        {
            // Create a copy of the original query so the original values don't get changed
            RouteValueDictionary routeValues = new RouteValueDictionary(originalQuery);

            // Update the route value for page to the page for this request
            routeValues[nameof(PagingQuery.Page)] = page;

            return urlHelper.Action(urlHelper.RequestContext.RouteData.Values[RouteValueKeys.Action].ToString(), urlHelper.RequestContext.RouteData.Values[RouteValueKeys.Controller].ToString(), routeValues: routeValues);
        }
    }
}
