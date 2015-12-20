using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using CH.Test.Mocks;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.ActionResults;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using FluentAssertions;
using Moq;
using TOTD.Mvc;

namespace CH.Test.ControllerTests.TestHelpers
{
    public static class ControllerTester
    {
        public static ControllerTester<ControllerType> UsingController<ControllerType>() where ControllerType : BaseController
        {
            Mock<IQueryDispatcher> mockQueryDispatcher = new Mock<IQueryDispatcher>(MockBehavior.Strict); // Only what is mocked in the test should be called
            Mock<ICommandDispatcher> mockCommandDispatcher = new Mock<ICommandDispatcher>(MockBehavior.Strict); // Only what is mocked in the test should be called
            MockHttpContext mockHttpContext = new MockHttpContext();

            ControllerType controller = (ControllerType)Activator.CreateInstance(typeof(ControllerType), mockQueryDispatcher.Object, mockCommandDispatcher.Object);

            RouteCollection routeCollection = new RouteCollection();
            routeCollection.Add(new Route("{controller}/{action}/{id}", null) { Defaults = new RouteValueDictionary(new { id = "defaultid" }) });

            controller.ControllerContext = new ControllerContext(mockHttpContext.Object.Request.RequestContext, controller);
            controller.Url = new UrlHelper(mockHttpContext.Object.Request.RequestContext, routeCollection);

            string controllerRoute = typeof(ControllerType).Name;
            controllerRoute = controllerRoute.Substring(0, controllerRoute.Length - "Controller".Length);
            controller.RouteData.Values.Add("controller", controllerRoute);

            return new ControllerTester<ControllerType>(controller, mockQueryDispatcher, mockCommandDispatcher, mockHttpContext);
        }
    }

    public class ControllerTester<ControllerType> where ControllerType : BaseController
    {
        private ActionResult _actionResult;

        private Action<Mock<IQueryDispatcher>> _verifyQueryDispatcher;
        private Action<Mock<ICommandDispatcher>> _verifyCommandDispatcher;

        public ControllerTester(ControllerType controller, Mock<IQueryDispatcher> mockQueryDispatcher, Mock<ICommandDispatcher> mockCommandDispatcher, MockHttpContext mockHttpContext)
        {
            this.Controller = controller;
            this.MockQueryDispatcher = mockQueryDispatcher;
            this.MockCommandDispatcher = mockCommandDispatcher;
            this.MockHttpContext = mockHttpContext;
        }

        public MockHttpContext MockHttpContext
        {
            get;
            private set;
        }

        public Mock<IQueryDispatcher> MockQueryDispatcher
        {
            get;
            private set;
        }

        public Mock<ICommandDispatcher> MockCommandDispatcher
        {
            get;
            private set;
        }

        public ControllerType Controller
        {
            get;
            private set;
        }

        public ControllerTester<ControllerType> ShouldHaveValidModelState()
        {
            Controller.ModelState.IsValid.Should().BeTrue();
            return this;
        }

        public ControllerTester<ControllerType> ShouldNotBeAjaxRequest()
        {
            Controller.Request.IsAjaxRequest().Should().BeFalse();
            return this;
        }

        public ControllerTester<ControllerType> ShouldBeAjaxRequest()
        {
            Controller.Request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            Controller.Request.IsAjaxRequest().Should().BeTrue();
            return this;
        }

        public ControllerTester<ControllerType> WithInvalidModelState(string errorMessage)
        {
            Controller.ModelState.AddModelError("", errorMessage);
            Controller.ModelState.IsValid.Should().BeFalse();
            return this;
        }

        public ControllerTester<ControllerType> ShouldHaveSingleModelError(string errorMessage)
        {
            ModelState modelState = Controller.ModelState[""];
            modelState.Errors.Should().HaveCount(1);
            modelState.Errors[0].ErrorMessage.Should().Be(errorMessage);
            return this;
        }

        public ControllerTester<ControllerType> SetupQueryDispatcher<ModelType>(ModelType model)
        {
            MockQueryDispatcher.Setup(x => x.Dispatch(It.IsAny<IQuery<ModelType>>())).Returns(model);
            _verifyQueryDispatcher = (mock) => mock.Verify(x => x.Dispatch(It.IsAny<IQuery<ModelType>>()), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupQueryDispatcher<ModelType>(ModelType model, IQuery<ModelType> query)
        {
            MockQueryDispatcher.Setup(x => x.Dispatch(query)).Returns(model);
            _verifyQueryDispatcher = (mock) => mock.Verify(x => x.Dispatch(query), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupQueryDispatcherAsync<ModelType>(ModelType model)
        {
            MockQueryDispatcher.Setup(x => x.DispatchAsync(It.IsAny<IAsyncQuery<ModelType>>())).Returns(Task.FromResult(model));
            _verifyQueryDispatcher = (mock) => mock.Verify(x => x.DispatchAsync(It.IsAny<IAsyncQuery<ModelType>>()), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupQueryDispatcherAsync<ModelType>(ModelType model, IAsyncQuery<ModelType> query)
        {
            MockQueryDispatcher.Setup(x => x.DispatchAsync(query)).Returns(Task.FromResult(model));
            _verifyQueryDispatcher = (mock) => mock.Verify(x => x.DispatchAsync(query), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupCommandDispatcherForSuccess(object command)
        {
            MockCommandDispatcher.Setup(x => x.Dispatch(command)).Returns(CommandResult.Success());
            _verifyCommandDispatcher = (mock) => mock.Verify(x => x.Dispatch(command), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupCommandDispatcherForSuccess<CommandType>() where CommandType : class
        {
            MockCommandDispatcher.Setup(x => x.Dispatch(It.IsAny<CommandType>())).Returns(CommandResult.Success());
            _verifyCommandDispatcher = (mock) => mock.Verify(x => x.Dispatch(It.IsAny<CommandType>()), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupCommandDispatcherForSuccessAsync<CommandType>() where CommandType : class
        {
            MockCommandDispatcher.Setup(x => x.DispatchAsync(It.IsAny<CommandType>())).Returns(Task.FromResult(CommandResult.Success()));
            _verifyCommandDispatcher = (mock) => mock.Verify(x => x.DispatchAsync(It.IsAny<CommandType>()), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupCommandDispatcherForSuccessAsync(object command)
        {
            MockCommandDispatcher.Setup(x => x.DispatchAsync(command)).Returns(Task.FromResult(CommandResult.Success()));
            _verifyCommandDispatcher = (mock) => mock.Verify(x => x.DispatchAsync(command), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupCommandDispatcherForFailure(object command, string errorMessage)
        {
            MockCommandDispatcher.Setup(x => x.Dispatch(command)).Returns(CommandResult.Failed(errorMessage));
            _verifyCommandDispatcher = (mock) => mock.Verify(x => x.Dispatch(command), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> SetupCommandDispatcherForFailureAsync(object command, string errorMessage)
        {
            MockCommandDispatcher.Setup(x => x.DispatchAsync(command)).Returns(Task.FromResult(CommandResult.Failed(errorMessage)));
            _verifyCommandDispatcher = (mock) => mock.Verify(x => x.DispatchAsync(command), Times.Once);

            return this;
        }

        public ControllerTester<ControllerType> WithCallTo(Expression<Func<ControllerType, ActionResult>> controllerAction)
        {
            _actionResult = controllerAction.Compile().Invoke(Controller);
            return this;
        }

        public ControllerTester<ControllerType> WithCallTo(Expression<Func<ControllerType, Task<ActionResult>>> controllerAction)
        {
            _actionResult = controllerAction.Compile().Invoke(Controller).Result;
            return this;
        }

        public ControllerTester<ControllerType> VerifyQueryDispatcher()
        {
            _verifyQueryDispatcher(MockQueryDispatcher);
            return this;
        }

        public ControllerTester<ControllerType> VerifyCommandDispatcher()
        {
            _verifyCommandDispatcher(MockCommandDispatcher);
            return this;
        }

        public ViewResultTester<ViewResult> ShouldReturnViewResult()
        {
            ViewResult viewResult = VerifyActionResult<ViewResult>();
            return new ViewResultTester<ViewResult>(viewResult);
        }

        public ViewResultTester<PartialViewResult> ShouldReturnPartialViewResult()
        {
            PartialViewResult viewResult = VerifyActionResult<PartialViewResult>();
            return new ViewResultTester<PartialViewResult>(viewResult);
        }

        public JsonCamelCaseResultTester ShouldReturnJsonCamelCase()
        {
            JsonCamelCaseResult jsonResult = VerifyActionResult<JsonCamelCaseResult>();
            return new JsonCamelCaseResultTester(jsonResult);
        }

        public RedirectToLocalResultTester ShouldRedirectToLocal()
        {
            RedirectToLocalResult redirectResult = VerifyActionResult<RedirectToLocalResult>();
            return new RedirectToLocalResultTester(redirectResult);
        }

        public RedirectToRouteResultTester ShouldRedirectToRoute()
        {
            RedirectToRouteResult redirectResult = VerifyActionResult<RedirectToRouteResult>();
            return new RedirectToRouteResultTester(redirectResult);
        }

        public ControllerTester<ControllerType> ShouldRedirectToPrevious()
        {
            VerifyActionResult<RedirectToPreviousResult>();
            return this;
        }

        public ControllerTester<ControllerType> ShouldReturnStatusCode(HttpStatusCode statusCode)
        {
            HttpStatusCodeResult statusResult = VerifyActionResult<HttpStatusCodeResult>();
            statusResult.StatusCode.Should().Be((int)statusCode);
            return this;
        }

        public ControllerTester<ControllerType> ShouldReturnStatusCode(HttpStatusCode statusCode, string statusDescription)
        {
            HttpStatusCodeResult statusResult = VerifyActionResult<HttpStatusCodeResult>();
            statusResult.StatusCode.Should().Be((int)statusCode);
            statusResult.StatusDescription.Should().Be(statusDescription);
            return this;
        }

        private ActionResultType VerifyActionResult<ActionResultType>() where ActionResultType : ActionResult
        {
            ActionResultType result = _actionResult as ActionResultType;
            result.Should().NotBeNull("expected " + typeof(ActionResultType).Name + " to be returned");
            return result;
        }
    }
}
