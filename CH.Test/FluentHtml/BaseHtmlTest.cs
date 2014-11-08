using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.StringHelpers;

namespace CH.Test.FluentHtml
{
    public class BaseHtmlTest
    {
        public ViewContext GetViewContext()
        {
            ViewContext result = new ViewContext();

            result.RouteData.Values["controller"] = "test";
            result.RouteData.Values["action"] = "action";
            result.Writer = new StringWriter();
            result.HttpContext = GetHttpContext();
            result.RequestContext = new RequestContext(GetHttpContext(), result.RouteData);

            return result;
        }

        public RouteCollection GetRouteCollection()
        {
            RouteCollection result = new RouteCollection();
            result.Add(new Route("{controller}/{action}/{id}", null)
            {
                Defaults = new RouteValueDictionary(new
                {
                    id = "defaultid"
                })
            });
            return result;
        }

        public HtmlHelper GetHtmlHelper()
        {
            return new HtmlHelper(GetViewContext(), GetViewDataContainer(new ViewDataDictionary()), GetRouteCollection());
        }

        public HtmlHelper<TModel> GetHtmlHelper<TModel>(ViewDataDictionary<TModel> viewData)
        {
            ViewContext viewContext = GetViewContext();
            viewContext.ViewData = viewData;

            IViewDataContainer container = GetViewDataContainer(viewData);

            return new HtmlHelper<TModel>(viewContext, container, GetRouteCollection());
        }

        public static IViewDataContainer GetViewDataContainer(ViewDataDictionary viewData)
        {
            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);
            return mockContainer.Object;
        }

        public HttpContextBase GetHttpContext()
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            Uri uri = new Uri("http://localhost");
            mockHttpContext.Setup(o => o.Request.Url).Returns(uri);

            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(String.Empty);

            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());
            return mockHttpContext.Object;
        }
    }

    public class TestController : Controller
    {
        public ActionResult TestAction(int actionID)
        {
            return null;
        }

        public ActionResult FormAction(string returnUrl)
        {
            return null;
        }
    }
}
