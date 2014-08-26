using System;
using System.Web.Mvc;
using System.Web.Routing;
using AR.Website.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test
{
    [TestClass]
    public class ActionHelperTests
    {
        [TestMethod]
        public void DoesNotOverrideAreaIfAlreadyExists()
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            routeValues.Add("area", "NoArea");

            ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestController>(x => x.TestAction(1), routeValues);

            Assert.AreEqual("NoArea", helperResult.RouteValues["area"]);
        }

        [TestMethod]
        public void GetsAreaFromAttributeOnController()
        {
            ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestController>(x => x.TestAction(1));
            Assert.AreEqual("TestArea", helperResult.RouteValues["area"]);
        }

        [TestMethod]
        public void SetsAreaToEmptyStringByDefault()
        {
            ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestControllerWithoutArea>(x => x.OtherAction());
            Assert.AreEqual("", helperResult.RouteValues["area"]);
        }

        [TestMethod]
        public void CanGetControllerNameWithoutControllerSuffixFromActionClass()
        {
            ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestController>(x => x.TestAction(1));
            Assert.AreEqual("Test", helperResult.ControllerName);
        }

        [TestMethod]
        public void CanGetActionNameFromMethod()
        {
            ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestController>(x => x.TestAction(1));
            Assert.AreEqual("TestAction", helperResult.ActionName);
        }

        [TestMethod]
        public void AddsMethodParametersToRouteValues()
        {
            ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestController>(x => x.TestAction(1));
            Assert.IsTrue(helperResult.RouteValues.ContainsKey("actionID"));
            object value = helperResult.RouteValues["actionID"];
            Assert.AreEqual(1, value);
        }

        [RouteArea("TestArea")]
        public class TestController : Controller
        {
            public ActionResult TestAction(int actionID)
            {
                return null;
            }

            public ActionResult DifferentAction(string test)
            {
                return null;
            }
        }

        public class TestControllerWithoutArea : Controller
        {
            public ActionResult OtherAction()
            {
                return null;
            }
        }
    }
}
