using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using CritterHeroes.Web.Middleware;

namespace CritterHeroes.Web.Areas.Common.ActionExtensions
{
    public static class MiscExtensions
    {
        public static string Picture(this UrlHelper urlHelper)
        {
            return urlHelper.Content($"~/{ImageMiddleware.Route}");
        }

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
    }
}
