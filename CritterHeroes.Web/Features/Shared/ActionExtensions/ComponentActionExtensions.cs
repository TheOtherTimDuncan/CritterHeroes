using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CritterHeroes.Web.Features.Components;

namespace CritterHeroes.Web.Features.Shared.ActionExtensions
{
    public static class ComponentActionExtensions
    {
        public static MvcHtmlString RenderCancelButton(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Action(nameof(ComponentsController.CancelButton), ComponentsController.Route, AreaName.NoAreaRouteValue);
        }

        public static string ComponentImageNotFoundAction(this UrlHelper urlHelper)
        {
            return urlHelper.Action("ImageNotFound", ComponentsController.Route, AreaName.NoAreaRouteValue);
        }
    }
}
