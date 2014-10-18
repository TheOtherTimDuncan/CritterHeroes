using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class FormElement : BaseContainerElement<FormElement>
    {
        public FormElement(HtmlHelper htmlHelper)
            : base(HtmlTag.Form, htmlHelper)
        {
            // Set defaults
            Method(FormMethod.Post);
            Action(this.CurrentActionName, this.CurrentControllerName);
        }

        public FormElement Action<T>(Expression<Func<T, ActionResult>> actionSelector) where T : IController
        {
            ActionHelperResult actionResult = ActionHelper.GetRouteValues(actionSelector);
            string url = UrlHelper.Action(actionResult.ActionName, actionResult.ControllerName, actionResult.RouteValues);
            return Action(url);
        }

        public FormElement Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, null);
        }

        public FormElement Action(string actionName, string controllerName, object routeValues)
        {
            string url = UrlHelper.Action(actionName, controllerName, routeValues);
            return Action(url);
        }

        public FormElement Action(string url)
        {
            Builder.MergeAttribute(HtmlAttribute.Action, url, replaceExisting: true);
            return this;
        }

        public FormElement Method(FormMethod formMethod)
        {
            Builder.MergeAttribute(HtmlAttribute.Method, HtmlHelper.GetFormMethodString(formMethod), replaceExisting: true);
            return this;
        }

        public FormElement AutoCompleteOff()
        {
            Builder.MergeAttribute(HtmlAttribute.AutoComplete, "off");
            return this;
        }
    }
}