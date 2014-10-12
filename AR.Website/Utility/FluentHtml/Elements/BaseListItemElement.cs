﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Html;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class BaseListItemElement<T> : BaseContainerElement<T> where T : BaseListItemElement<T>
    {
        public BaseListItemElement(HtmlHelper htmlHelper)
            : base(HtmlTag.ListItem, htmlHelper)
        {
        }

        public T Text(string text)
        {
            AddInnerHtml(text);
            return (T)this;
        }
    }
}