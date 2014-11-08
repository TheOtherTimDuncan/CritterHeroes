using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Contracts;
using CH.Website.Utility.FluentHtml.Elements;
using TOTD.Utility.StringHelpers;

namespace CH.Website.Utility.FluentHtml.Bootstrap
{
    public class BootstrapNavItem : BaseListItemElement<BootstrapNavItem>
    {
        private bool _setActive;

        public BootstrapNavItem(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {
            this._setActive = false;
        }

        public BootstrapNavItem AsDropdown(string text)
        {
            return this.Class("dropdown")
                .AddElement
                (
                    CreateElement<LinkElement>()
                        .Class("dropdown-toggle")
                        .Data("toggle", "dropdown")
                        .AsJavascriptLink()
                        .Text(text)
                        .AddElement
                        (
                            CreateElement<SpanElement>()
                                .Class("caret")
                        )
                );
        }

        public BootstrapNavItem SetActiveByControllerAndAction()
        {
            this._setActive = true;
            return this;
        }

        protected override void PreRender()
        {
            base.PreRender();

            if (this._setActive)
            {
                InternalUrlContext urlContext = new InternalUrlContext(this.CurrentActionName, this.CurrentControllerName, this.CurrentArea);
                if (FindMatchingUrlContext(this.Children, urlContext))
                {
                    Builder.AddCssClass("active");
                }
            }
        }

        private bool FindMatchingUrlContext(IEnumerable<IElement> elements, IUrlContext urlContext)
        {
            foreach (IElement element in elements)
            {
                IUrlContext elementUrlContext = element as IUrlContext;
                if (elementUrlContext != null && elementUrlContext.Matches(urlContext))
                {
                    return true;
                }

                IContainerElement container = element as IContainerElement;
                if (container != null)
                {
                    if (FindMatchingUrlContext(container.Children, urlContext))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}