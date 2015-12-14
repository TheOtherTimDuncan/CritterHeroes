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
using CritterHeroes.Web.Common.Commands;
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
        public void SetupDependencies()
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

        public async Task<ResultType> TestControllerAsync<ControllerType, ResultType>(Func<ControllerType, Task<ActionResult>> test)
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

        public ResultType TestControllerGet<ControllerType, ResultType, ModelType>(IQuery<ModelType> query, ModelType model, Func<ControllerType, ActionResult> test)
           where ControllerType : BaseController
           where ResultType : ActionResult
        {
            mockQueryDispatcher.Setup(x => x.Dispatch(query)).Returns(model);
            ResultType result = TestController<ControllerType, ResultType>(test);
            mockQueryDispatcher.Verify(x => x.Dispatch(query), Times.Once);
            return result;
        }

        public async Task<ResultType> TestControllerGetAsync<ControllerType, ResultType, ModelType>(IAsyncQuery<ModelType> query, ModelType model, Func<ControllerType, Task<ActionResult>> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            mockQueryDispatcher.Setup(x => x.DispatchAsync(query)).Returns(Task.FromResult(model));
            ResultType result = await TestControllerAsync<ControllerType, ResultType>(test);
            mockQueryDispatcher.Verify(x => x.DispatchAsync(query), Times.Once);
            return result;
        }

        public ResultType TestControllerPostSuccessWithValidModelState<ControllerType, ResultType>(object command, Func<ControllerType, ActionResult> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            mockCommandDispatcher.Setup(x => x.Dispatch(command)).Returns(CommandResult.Success());

            ResultType result = TestController<ControllerType, ResultType>((controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();
                return test(controller);
            });

            mockCommandDispatcher.Verify(x => x.Dispatch(command), Times.Once);

            return result;
        }

        public async Task<ResultType> TestControllerPostSuccessWithValidModelStateAsync<ControllerType, ResultType>(object command, Func<ControllerType, Task<ActionResult>> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            mockCommandDispatcher.Setup(x => x.DispatchAsync(command)).Returns(Task.FromResult(CommandResult.Success()));

            ResultType result = await TestControllerAsync<ControllerType, ResultType>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();
                return await test(controller);
            });

            mockCommandDispatcher.Verify(x => x.DispatchAsync(command), Times.Once);

            return result;
        }

        public ResultType TestControllerPostFailWithValidModelState<ControllerType, ResultType>(object command, string errorMessage, Func<ControllerType, ActionResult> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            mockCommandDispatcher.Setup(x => x.Dispatch(command)).Returns(CommandResult.Failed(errorMessage));

            ResultType result = TestController<ControllerType, ResultType>((controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();
                ActionResult actionResult = test(controller);

                ModelState modelState = controller.ModelState[""];
                modelState.Errors.Should().HaveCount(1);
                modelState.Errors[0].ErrorMessage.Should().Be(errorMessage);

                return actionResult;
            });

            mockCommandDispatcher.Verify(x => x.Dispatch(command), Times.Once);

            return result;
        }

        public async Task<ResultType> TestControllerPostFailWithValidModelStateAsync<ControllerType, ResultType>(object command, string errorMessage, Func<ControllerType, Task<ActionResult>> test)
           where ControllerType : BaseController
           where ResultType : ActionResult
        {
            mockCommandDispatcher.Setup(x => x.DispatchAsync(command)).Returns(Task.FromResult(CommandResult.Failed(errorMessage)));

            ResultType result = await TestControllerAsync<ControllerType, ResultType>(async (controller) =>
            {
                controller.ModelState.IsValid.Should().BeTrue();
                ActionResult actionResult = await test(controller);

                ModelState modelState = controller.ModelState[""];
                modelState.Errors.Should().HaveCount(1);
                modelState.Errors[0].ErrorMessage.Should().Be(errorMessage);

                return actionResult;
            });

            mockCommandDispatcher.Verify(x => x.DispatchAsync(command), Times.Once);

            return result;
        }

        public ResultType TestControllerPostWithInvalidModelState<ControllerType, ResultType>(string errorMessage, Func<ControllerType, ActionResult> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            ResultType result = TestController<ControllerType, ResultType>((controller) =>
            {
                controller.ModelState.AddModelError("", errorMessage);
                controller.ModelState.IsValid.Should().BeFalse();
                return test(controller);
            });

            return result;
        }

        public async Task<ResultType> TestControllerPostWithInvalidModelStateAsync<ControllerType, ResultType>(string errorMessage, Func<ControllerType, Task<ActionResult>> test)
            where ControllerType : BaseController
            where ResultType : ActionResult
        {
            ResultType result = await TestControllerAsync<ControllerType, ResultType>(async (controller) =>
            {
                controller.ModelState.AddModelError("", errorMessage);
                controller.ModelState.IsValid.Should().BeFalse();
                return await test(controller);
            });

            return result;
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
