using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.CommandHandlers;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class UserActionCommandHandlerTests
    {
        [TestMethod]
        public void ThrowsExceptionIfHandlerDoesNotImplementCorrectInterface()
        {
            Mock<IAsyncCommandHandler<LoginModel>> mockHandler = new Mock<IAsyncCommandHandler<LoginModel>>();

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Action action = () =>
            {
                UserActionCommandHandler<LoginModel> handler = new UserActionCommandHandler<LoginModel>(mockHandler.Object, mockUserLogger.Object);
            };

            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("handler is not of type IAsyncCommandHandler");
        }

        [TestMethod]
        public async Task LogsUserActionWithSuccessActionIfDecoratedHandlerSucceeds()
        {
            LoginModel model = new LoginModel()
            {
                Username = "username"
            };

            UserActions actionSuccess = UserActions.PasswordLoginSuccess;

            Mock<IAsyncUserCommandHandler<LoginModel>> mockHandler = new Mock<IAsyncUserCommandHandler<LoginModel>>();
            mockHandler.Setup(x => x.ExecuteAsync(model)).Returns(Task.FromResult(CommandResult.Success()));
            mockHandler.Setup(x => x.SuccessUserAction).Returns(actionSuccess);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            mockUserLogger.Setup(x => x.LogActionAsync(actionSuccess, model.Username)).Returns(Task.FromResult(0));

            UserActionCommandHandler<LoginModel> handler = new UserActionCommandHandler<LoginModel>(mockHandler.Object, mockUserLogger.Object);
            await handler.ExecuteAsync(model);

            mockHandler.Verify(x => x.ExecuteAsync(model), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(actionSuccess, model.Username), Times.Once);
        }

        [TestMethod]
        public async Task LogsUserActionWithFailureActionIfDecoratedHandlerFails()
        {
            LoginModel model = new LoginModel()
            {
                Username = "username"
            };

            UserActions actionFailure = UserActions.PasswordLoginFailure;

            Mock<IAsyncUserCommandHandler<LoginModel>> mockHandler = new Mock<IAsyncUserCommandHandler<LoginModel>>();
            mockHandler.Setup(x => x.ExecuteAsync(model)).Returns(Task.FromResult(CommandResult.Failed("", "error")));
            mockHandler.Setup(x => x.FailedUserAction).Returns(actionFailure);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            mockUserLogger.Setup(x => x.LogActionAsync(actionFailure, model.Username, model)).Returns(Task.FromResult(0));

            UserActionCommandHandler<LoginModel> handler = new UserActionCommandHandler<LoginModel>(mockHandler.Object, mockUserLogger.Object);
            await handler.ExecuteAsync(model);

            mockHandler.Verify(x => x.ExecuteAsync(model), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(actionFailure, model.Username, model), Times.Once);
        }
    }
}
