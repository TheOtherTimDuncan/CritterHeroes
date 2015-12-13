using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using CH.Test.Mocks;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Mvc;
using TOTD.Mvc.FluentHtml;

namespace CH.Test.ControllerTests
{
    public class BaseControllerTest : BaseTest
    {
        public MockHttpContext mockHttpContext;

        public Mock<IQueryDispatcher> mockQueryDispatcher;
        public Mock<ICommandDispatcher> mockCommandDispatcher;

        [TestInitialize]
        public void SetupDependencyContainer()
        {
            mockQueryDispatcher = new Mock<IQueryDispatcher>(MockBehavior.Strict); // Only what is mocked in the test should be called
            mockCommandDispatcher = new Mock<ICommandDispatcher>(MockBehavior.Strict); // Only what is mocked in the test should be called
            mockHttpContext = new MockHttpContext();
        }

        public T CreateController<T>() where T : BaseController
        {
            T controller = (T)Activator.CreateInstance(typeof(T), mockQueryDispatcher.Object, mockCommandDispatcher.Object);

            RouteCollection routeCollection = new RouteCollection();
            routeCollection.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });

            controller.ControllerContext = new ControllerContext(mockHttpContext.Object.Request.RequestContext, controller);
            controller.Url = new UrlHelper(mockHttpContext.Object.Request.RequestContext, routeCollection);

            string controllerRoute = typeof(T).Name;
            controllerRoute = controllerRoute.Substring(0, controllerRoute.Length - "Controller".Length);
            controller.RouteData.Values.Add("controller", controllerRoute);

            return controller;
        }

        public ResultType TestController<ControllerType, ResultType>(Func<ControllerType, ActionResult> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            using (ControllerType controller = CreateController<ControllerType>())
            {
                ResultType result = test(controller) as ResultType;
                result.Should().NotBeNull("expected " + typeof(ResultType).Name + " to be returned");
                return result;
            }
        }

        public async Task<ResultType> TestController<ControllerType, ResultType>(Func<ControllerType, Task<ActionResult>> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            using (ControllerType controller = CreateController<ControllerType>())
            {
                ResultType result = (await test(controller)) as ResultType;
                result.Should().NotBeNull("expected " + typeof(ResultType).Name + " to be returned");
                return result;
            }
        }

        public void VerifyRedirectToRouteResult(RedirectToRouteResult redirectResult, string actionName, string controllerName)
        {
            redirectResult.Should().NotBeNull();
            redirectResult.RouteValues[RouteValueKeys.Controller].Should().Be(controllerName);
            redirectResult.RouteValues[RouteValueKeys.Action].Should().Be(actionName);
        }

        public T GetJsonResultValue<T>(JsonResult jsonResult, string key)
        {
            return GetPropertyValue<T>(jsonResult.Data, key);
        }

        public T GetJsonResultValue<T>(JsonCamelCaseResult jsonResult, string key)
        {
            return GetPropertyValue<T>(jsonResult.Data, key);
        }

        private T GetPropertyValue<T>(object data, string key)
        {
            PropertyInfo propertyInfo = data.GetType().GetProperty(key);
            object propertyValue = propertyInfo.GetValue(data, null);
            propertyValue.Should().NotBeNull(key + " should exist in " + data.GetType().Name);

            if (propertyValue is IEnumerable)
            {
                return (T)propertyValue;
            }

            return (T)Convert.ChangeType(propertyValue, typeof(T));
        }
    }
}
