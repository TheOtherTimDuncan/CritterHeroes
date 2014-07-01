using System;
using AR.Website.Utility.FluentHtml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.FluentHtml
{
    [TestClass]
    public class UrlContextTests
    {
        [TestClass]
        public class InternalUrlContextTests
        {
            [TestMethod]
            public void CorrectlyParsesAreaNameFromRouteValues()
            {
                InternalUrlContext urlContext = new InternalUrlContext("action", "controller", new
                {
                    area = "area"
                });
                Assert.AreEqual("area", urlContext.Area);
                Assert.AreEqual("controller", urlContext.ControllerName);
                Assert.AreEqual("action", urlContext.ActionName);
            }

            [TestMethod]
            public void MatchesToContextWithoutArea()
            {
                InternalUrlContext context1 = new InternalUrlContext("action", "controller");
                InternalUrlContext context2 = new InternalUrlContext("action", "controller");
                Assert.IsTrue(context1.Matches(context2));
            }
        }
    }
}
