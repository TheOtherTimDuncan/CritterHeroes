using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class LinkElement : Element<LinkElement>
    {
        private UrlHelper helperUrl;

        public LinkElement(UrlHelper urlHelper)
            :base("a")
        {
            this.helperUrl = urlHelper;
        }

        public LinkElement Url(string actionName, string controllerName)
        {
            return Url(actionName, controllerName, null);
        }

        public LinkElement Url(string actionName, string controllerName, object routeValues)
        {
            string url = helperUrl.Action(actionName, controllerName, routeValues);
            return this.Attribute("href", url);
        }

        public LinkElement Text(string text)
        {
            Builder.InnerHtml = text;
            return this;
        }
    }
}