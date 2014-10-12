using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

        public HtmlHelper GetHtmlHelper()
        {
            return new HtmlHelper(GetViewContext(), GetViewDataContainer(null));
        }

        public HtmlHelper<TModel> GetHtmlHelper<TModel>(ViewDataDictionary<TModel> viewData)
        {
            Mock<ViewContext> mockViewContext = new Mock<ViewContext>()
            {
                CallBase = true
            };

            RouteData routeData = new RouteData();
            routeData.Values["controller"] = "controller";
            routeData.Values["action"] = "action";

            mockViewContext.Setup(x => x.ViewData).Returns(viewData);
            mockViewContext.Setup(x => x.HttpContext.Items).Returns(new Hashtable());
            mockViewContext.Setup(x => x.RouteData).Returns(routeData);

            IViewDataContainer container = GetViewDataContainer(viewData);

            return new HtmlHelper<TModel>(mockViewContext.Object, container);
        }

        public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
        {
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }
    }
}
