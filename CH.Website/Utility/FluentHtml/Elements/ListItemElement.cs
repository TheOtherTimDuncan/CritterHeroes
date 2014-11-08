using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CH.Website.Utility.FluentHtml.Contracts;

namespace CH.Website.Utility.FluentHtml.Elements
{
    public class ListItemElement : BaseListItemElement<ListItemElement>
    {
        public ListItemElement(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {
        }
    }
}