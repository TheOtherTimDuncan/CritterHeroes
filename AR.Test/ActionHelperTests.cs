using System;
using System.Diagnostics;
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

        //[TestMethod]
        public void TestTiming()
        {
            Stopwatch watch = Stopwatch.StartNew();
            int max = 1000000;
            for (var i = 1; i <= max; i++)
            {
                ActionHelperResult helperResult = ActionHelper.GetRouteValues<TestController>(x => x.TestAction(1));
            }
            watch.Stop();
            Console.WriteLine("Total time: {0}; Average time: {1}", watch.ElapsedMilliseconds, (double)watch.ElapsedMilliseconds / max);
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
