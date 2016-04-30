using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Features.Account.CommandHandlers;
using CritterHeroes.Web.Features.Account.Models;
using FluentAssertions;
using Microsoft.Owin.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class LogoutCommandHandlerTests
    {
        [TestMethod]
        public void LogsOutUserAndClearsUserState()
        {
            Mock<IAuthenticationManager> mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(x => x.SignOut());

            Mock<IStateManager<UserContext>> mockStateManager = new Mock<IStateManager<UserContext>>();
            mockStateManager.Setup(x => x.ClearContext());

            LogoutCommandHandler handler = new LogoutCommandHandler(mockAuthenticationManager.Object, mockStateManager.Object);
            handler.Execute(new LogoutModel());

            mockAuthenticationManager.Verify(x => x.SignOut(), Times.Once);
            mockStateManager.Verify(x => x.ClearContext(), Times.Once);
        }
    }
}
