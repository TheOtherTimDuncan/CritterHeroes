using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class LinkElement : BaseContainerElement<LinkElement>, IUrlContext
    {
        private IUrlContext _urlContext;

        public LinkElement(ViewContext viewContext)
            : base("a", viewContext)
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
            return this.Attribute("href", "#");
        }

        public LinkElement Text(string text)
        {
            AddInnerHtml(text);
            return this;
        }

        private void SetUrl(string url)
        {
            this.Attribute("href", url);
        }
    }
}