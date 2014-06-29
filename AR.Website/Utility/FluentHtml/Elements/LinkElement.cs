using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class LinkElement : BaseContainerElement<LinkElement>
    {
        public LinkElement(ViewContext viewContext)
            : base("a", viewContext)
        {
        }

        public LinkElement ActionLink(string actionName, string controllerName)
        {
            return ActionLink(actionName, controllerName, null);
        }

        public LinkElement ActionLink(string actionName, string controllerName, object routeValues)
        {
            string url = UrlHelper.Action(actionName, controllerName, routeValues);
            return this.Url(url);
        }

        public LinkElement Url(string url)
        {
            return this.Attribute("href", url);
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
    }
}