using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Contracts;
using CH.Website.Utility.FluentHtml.Html;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class LinkElement : BaseContainerElement<LinkElement>, IUrlContext
    {
        private IUrlContext _urlContext;

        public LinkElement(HtmlHelper htmlHelper)
            : base(HtmlTag.Link, htmlHelper)
        {
        }

        public bool Matches(IUrlContext urlContext)
        {
            if (this._urlContext == null)
            {
                return false;
            }

            return this._urlContext.Matches(urlContext);
        }

        public LinkElement ActionLink<T>(Expression<Func<T, Task<ActionResult>>> actionSelector) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<T>(actionSelector);

            _urlContext = new InternalUrlContext(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);

            string url = UrlHelper.Action(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
            SetUrl(url);

            return this;
        }

        public LinkElement ActionLink<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues<T>(actionSelector);

            _urlContext = new InternalUrlContext(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);

            string url = UrlHelper.Action(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
            SetUrl(url);

            return this;
        }

        public LinkElement ActionLink(string actionName, string controllerName)
        {
            return ActionLink(actionName, controllerName, null);
        }

        public LinkElement ActionLink(string actionName, string controllerName, object routeValues)
        {
            _urlContext = new InternalUrlContext(actionName, controllerName, routeValues);

            string url = UrlHelper.Action(actionName, controllerName, routeValues);
            SetUrl(url);

            return this;
        }

        public LinkElement Url(string url)
        {
            _urlContext = new ExternalUrlContext(url);
            SetUrl(url);
            return this;
        }

        public LinkElement AsJavascriptLink()
        {
            return this.Attribute(HtmlAttribute.HRef, "#");
        }

        public LinkElement Text(string text)
        {
            AddInnerHtml(text);
            return this;
        }

        private void SetUrl(string url)
        {
            this.Attribute(HtmlAttribute.HRef, url);
        }
    }
}