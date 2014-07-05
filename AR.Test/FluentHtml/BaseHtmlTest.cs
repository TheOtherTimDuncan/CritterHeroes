using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AR.Test.FluentHtml
{
    public class BaseHtmlTest
    {
        public ViewContext GetViewContext()
        {
            ViewContext result = new ViewContext();
            result.RouteData.Values["controller"] = "controller";
            result.RouteData.Values["action"] = "action";
            return result;
        }
    }
}
