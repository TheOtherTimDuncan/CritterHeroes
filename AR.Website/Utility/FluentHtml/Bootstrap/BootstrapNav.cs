using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Website.Utility.FluentHtml.Elements;

namespace AR.Website.Utility.FluentHtml.Bootstrap
{
    public class BootstrapNav : BaseListElement<BootstrapNav>
    {
        public BootstrapNav(ViewContext viewContext)
            : base(ListType.Unordered, viewContext)
        {
        }

        public BootstrapNav AsNavBar()
        {
            return Class("nav").Class("navbar-nav");

        }

        public BootstrapNav AsDropdown()
        {
            return Class("dropdown-menu");
        }
    }
}