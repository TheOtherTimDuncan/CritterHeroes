using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Contracts;

namespace AR.Website.Utility.FluentHtml.Elements
{
    public class ListItemElement : BaseListItemElement<ListItemElement>
    {
        public ListItemElement(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {
        }
    }
}